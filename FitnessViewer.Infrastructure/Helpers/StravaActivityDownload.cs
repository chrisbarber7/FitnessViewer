using System;
using FitnessViewer.Infrastructure.Repository;
using FitnessViewer.Infrastructure.Models;
using System.Collections.Generic;

using Newtonsoft.Json.Linq;
using System.Linq;

// Strava.DotNet
using StravaDotNetAthletes = Strava.Athletes;
using StravaDotNetAuthentication = Strava.Authentication;
using StravaDotNetClient = Strava.Clients;
using StravaDotNetStreams = Strava.Streams;
using StravaDotNetActivities = Strava.Activities;
using StravaDotNetApi = Strava.Api;
using StravaDotNetGear = Strava.Gear;
using AutoMapper;
using FitnessViewer.Infrastructure.enums;

namespace FitnessViewer.Infrastructure.Helpers
{
    public class StravaActivityDownload : Strava
    {
        public StravaActivityDownload(string userId) : base(userId)
        { }


        /// <summary>
        /// Download detailed information for given activity
        /// </summary>
        /// <param name="activityId">Strava activity Id</param>
        public void ActivityDetailsDownload(long activityId)
        {
            Models.Activity fvActivity = _unitOfWork.Activity.GetActivity(activityId);

            if (fvActivity == null)
                throw new ArgumentException("Activity Not Found");

            LogActivity("Download Activity", fvActivity);

            var stravaActivity = _client.Activities.GetActivity(activityId.ToString(), true);

            // update details missing from summary activity.
            fvActivity.Calories = Convert.ToDecimal(stravaActivity.Calories);
            fvActivity.Description = stravaActivity.Description;
            // gear??
            fvActivity.EmbedToken = stravaActivity.EmbedToken;
            fvActivity.DeviceName = stravaActivity.DeviceName;
            fvActivity.MapPolyline = stravaActivity.Map.Polyline;
            // splits_metric
            // splits_standard


            // heart rate/power zones
            //List<ActivityZone> zones = _client.Activities.GetActivityZones(activity.Id.ToString());


            LogActivity("Download Laps", fvActivity);
            foreach (StravaDotNetActivities.ActivityLap stravaLap in _client.Activities.GetActivityLaps(activityId.ToString()))
                _unitOfWork.Activity.AddLap(Mapper.Map<Lap>(stravaLap));


            LogActivity("Download Stream", fvActivity);

            // detailed information
            List<StravaDotNetStreams.ActivityStream> stream = _client.Streams.GetActivityStream(fvActivity.Id.ToString(),
                StravaDotNetStreams.StreamType.Altitude |
                StravaDotNetStreams.StreamType.Cadence |
                StravaDotNetStreams.StreamType.Distance |
                StravaDotNetStreams.StreamType.GradeSmooth |
                StravaDotNetStreams.StreamType.Heartrate |
                StravaDotNetStreams.StreamType.LatLng |
                StravaDotNetStreams.StreamType.Moving |
                StravaDotNetStreams.StreamType.Temperature |
                StravaDotNetStreams.StreamType.Time |
                StravaDotNetStreams.StreamType.Velocity_Smooth |
                StravaDotNetStreams.StreamType.Watts,
                StravaDotNetStreams.StreamResolution.All);

            ExtractAndStoreStream(fvActivity.Id, stream);

            if (fvActivity.ActivityType.IsRun)
                ExtractRunDetails(fvActivity, stravaActivity);
            else if (fvActivity.ActivityType.IsRide)
                ExtractBikeDetails(stravaActivity);

            _unitOfWork.Complete();

            if (stravaLimitDelay > 100)
                LogActivity(string.Format("Pausing for {0}ms", stravaLimitDelay.ToString()), fvActivity);

            System.Threading.Thread.Sleep(stravaLimitDelay);
        }

        public void Complete()
        {
            _unitOfWork.Complete();
        }

        /// <summary>
        /// Convert Strava stream information and write to database.
        /// </summary>
        /// <param name="activityId">Strava activity id</param>
        /// <param name="stream">Detailed information for activity in strava format</param>
        private void ExtractAndStoreStream(long activityId, List<StravaDotNetStreams.ActivityStream> stream)
        {
            List<Stream> convertedStream = new List<Stream>();

            // convert each strava item
            for (int row = 0; row <= stream[0].OriginalSize - 1; row++)
            {
                Stream s = ExtractStreamRow(activityId, stream, row);

                // occasinally we get duplicate rows for the same time back from Strava so need to ignore them!
                if (convertedStream.Where(c => c.Time == s.Time).Any())
                {
                    System.Diagnostics.Debug.WriteLine(string.Format("Skipping row {0} for activity {1}", s.Time, activityId));
                    continue;
                }                    

                convertedStream.Add(s);
            }


            List<ActivityPeakDetail> powerPeaks = PeakValueFinder.ExtractPeaksFromStream(activityId, convertedStream.Select(s => s.Watts).ToList(), PeakStreamType.Power);
            if (powerPeaks != null)
                _unitOfWork.Analysis.AddPeak(activityId, PeakStreamType.Power, powerPeaks);

            List<ActivityPeakDetail> cadencePeaks = PeakValueFinder.ExtractPeaksFromStream(activityId, convertedStream.Select(s => s.Cadence).ToList(), PeakStreamType.Cadence);
            if (cadencePeaks != null)
                _unitOfWork.Analysis.AddPeak(activityId, PeakStreamType.Cadence, cadencePeaks);

            List<ActivityPeakDetail> heartRatePeaks = PeakValueFinder.ExtractPeaksFromStream(activityId, convertedStream.Select(s => s.HeartRate).ToList(), PeakStreamType.HeartRate);
            if (heartRatePeaks != null)
                _unitOfWork.Analysis.AddPeak(activityId, PeakStreamType.HeartRate, heartRatePeaks);

            // write all details to database.
            _unitOfWork.Activity.AddStream(convertedStream);
        }



        /// <summary>
        /// Convert a single stream row.
        /// </summary>
        /// <param name="activityId">Strava activity id</param>
        /// <param name="stream">Detailed information for activity in strava format</param>
        /// <param name="rowNumber">Row to be processed</param>
        /// <returns></returns>
        private static Stream ExtractStreamRow(long activityId, List<StravaDotNetStreams.ActivityStream> stream, int rowNumber)
        {
            Stream newStream = new Stream();
            newStream.ActivityId = activityId;

            foreach (StravaDotNetStreams.ActivityStream s in stream)
            {
                switch (s.StreamType)
                {
                    case StravaDotNetStreams.StreamType.LatLng:
                        {
                            // convert json lat/long  
                            JArray o = (JArray)s.Data[rowNumber];
                            newStream.Latitude = (double)o.First;
                            newStream.Longitude = (double)o.Last;

                            break;
                        }
                    case StravaDotNetStreams.StreamType.Altitude: { newStream.Altitude = Convert.ToDouble(s.Data[rowNumber]); break; }
                    case StravaDotNetStreams.StreamType.Cadence: { newStream.Cadence = Convert.ToInt32(s.Data[rowNumber]); break; }
                    case StravaDotNetStreams.StreamType.Distance: { newStream.Distance = Convert.ToDouble(s.Data[rowNumber]); break; }
                    case StravaDotNetStreams.StreamType.GradeSmooth: { newStream.Gradient = Convert.ToDouble(s.Data[rowNumber]); break; }
                    case StravaDotNetStreams.StreamType.Heartrate: { newStream.HeartRate = Convert.ToInt32(s.Data[rowNumber]); break; }
                    case StravaDotNetStreams.StreamType.Moving: { newStream.Moving = Convert.ToBoolean(s.Data[rowNumber]); break; }
                    case StravaDotNetStreams.StreamType.Temperature: { newStream.Temperature = Convert.ToInt32(s.Data[rowNumber]); break; }
                    case StravaDotNetStreams.StreamType.Time: { newStream.Time = Convert.ToInt32(s.Data[rowNumber]); break; }
                    case StravaDotNetStreams.StreamType.Velocity_Smooth: { newStream.Velocity = Convert.ToDouble(s.Data[rowNumber]); break; }
                    case StravaDotNetStreams.StreamType.Watts: { newStream.Watts = Convert.ToInt32(s.Data[rowNumber]); break; }
                }
            }

            return newStream;
        }

        /// <summary>
        /// Download bike specific information
        /// </summary>
        /// <param name="activity">StravaActivity</param>
        private void ExtractBikeDetails(StravaDotNetActivities.Activity activity)
        {
        }

        /// <summary>
        /// Download run specific information
        /// </summary>
        /// <param name="activity">StravaActivity</param>
        private void ExtractRunDetails(Activity fvActivity, StravaDotNetActivities.Activity stravaActivity)
        {

            LogActivity("Download Best Efforts", fvActivity);
            foreach (StravaDotNetActivities.BestEffort effort in stravaActivity.BestEfforts)
                InsertBestEffort(fvActivity, effort);

            //   _unitOfWork.Complete();
        }

        /// <summary>
        /// Add best effort information to database.
        /// </summary>
        /// <param name="activityId">Strava activity Id</param>
        /// <param name="e">Strava best effort information</param>
        private void InsertBestEffort(Activity fvActivity, StravaDotNetActivities.BestEffort e)
        {
            BestEffort effort = new BestEffort();

            effort.ActivityId = fvActivity.Id;
            effort.MovingTime = TimeSpan.FromSeconds(e.MovingTime);
            effort.Name = e.Name;
            effort.ResourceState = e.ResourceState;
            effort.StartDate = DateTime.Parse(e.StartDate);
            effort.StartDateLocal = DateTime.Parse(e.StartDateLocal);
            effort.StartIndex = e.StartIndex;
            effort.ElapsedTime = TimeSpan.FromSeconds(e.ElapsedTime);
            effort.Distance = Convert.ToDecimal(e.Distance);
            effort.EndIndex = e.EndIndex;

            _unitOfWork.Activity.AddBestEffort(effort);
        }



    }
}
