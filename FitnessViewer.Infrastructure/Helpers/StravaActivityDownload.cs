using System;
using FitnessViewer.Infrastructure.Models;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.Linq;
using AutoMapper;
using FitnessViewer.Infrastructure.enums;
using FitnessViewer.Infrastructure.Models.Collections;
using FitnessViewer.Infrastructure.Interfaces;
using FitnessViewer.Infrastructure.Helpers.Analytics;

// Strava.DotNet
using StravaDotNetStreams = Strava.Streams;
using StravaDotNetActivities = Strava.Activities;

namespace FitnessViewer.Infrastructure.Helpers
{
    public class StravaActivityDownload : Strava
    {
        public StravaActivityDownload(IUnitOfWork uow, string userId) : base(uow, userId)
        { }
        
        private Models.Activity _fvActivity;
        private StravaDotNetActivities.Activity _stravaActivity;
        private long _activityId;
        private int? _streamSize = null;
        private ActivityStreams _convertedStream;

        /// <summary>
        /// Download detailed information for given activity
        /// </summary>
        /// <param name="activityId">Strava activity Id</param>
        public void ActivityDetailsDownload(long activityId)
        {
            _activityId = activityId;

            DeleteExistingActivityDetails(activityId);
            GetFitnessViewerActivity();
            GetStravaActivity();

            if (!_stravaActivity.IsManual)
            {
                DownloadLaps();
                DownloadZones();
                DownloadStream();
                ExtractRunDetails();
                ExtractBikeDetails();
            }

            UpdateActivityDetails();

            AddNotification(Notification.StravaActivityDownload(_activityId));
            StravaPause(_fvActivity);
        }

        private void DeleteExistingActivityDetails(long activityId)
        {
            if (!_unitOfWork.Activity.DeleteActivityDetails(activityId))
                throw new Exception("Error Details Existing Details.");
        }

        private void GetStravaActivity()
        {
            LogActivity("Download Activity From Strava", _fvActivity);
            _stravaActivity = _client.Activities.GetActivity(_activityId.ToString(), true);
        }

        private void GetFitnessViewerActivity()
        {
            _fvActivity = _unitOfWork.CRUDRepository.GetByKey<Activity>(_activityId, o => o.ActivityType, o => o.Athlete);

            if (_fvActivity == null)
                throw new ArgumentException(string.Format("Activity Not Found: {0}", _activityId.ToString()));
        }

        private void UpdateActivityDetails()
        {

            // update details missing from summary activity.
            _fvActivity.Calories = Convert.ToDecimal(_stravaActivity.Calories);
            _fvActivity.Description = _stravaActivity.Description;
            // gear??
            _fvActivity.EmbedToken = _stravaActivity.EmbedToken;
            _fvActivity.DeviceName = _stravaActivity.DeviceName;
            _fvActivity.MapPolyline = _stravaActivity.Map.Polyline;
            // splits_metric
            // splits_standard

            if ((_fvActivity.HasPowerMeter) && (_convertedStream.HasIndividualStream(StreamType.Watts)))
            {
                BikePower calc = new BikePower(_convertedStream.GetIndividualStream<int?>(enums.StreamType.Watts), 295);

                _fvActivity.TSS = calc.TSS();
                _fvActivity.IntensityFactor = calc.IntensityFactor();
            }

            if (_streamSize != null)
                _fvActivity.StreamSize = _streamSize;

            _fvActivity.DetailsDownloaded = true;

            _unitOfWork.Complete();
        }

        public void DownloadZones()
        {
            /*
            Option Disabled at the moment as run pace zones are causing errors which need investigating.
            */

            // heart rate/power zones
            //       List<StravaDotNetActivities.ActivityZone> zones = _client.Activities.GetActivityZones(_activityId.ToString());

        }

        public void DownloadStream()
        {
            LogActivity("Download Stream", _fvActivity);

            // detailed information
            List<StravaDotNetStreams.ActivityStream> stream = _client.Streams.GetActivityStream(_fvActivity.Id.ToString(),
                StravaDotNetStreams.StreamType.Altitude |
                StravaDotNetStreams.StreamType.Cadence |
                StravaDotNetStreams.StreamType.Distance |
                StravaDotNetStreams.StreamType.Grade_Smooth |
                StravaDotNetStreams.StreamType.Heartrate |
                StravaDotNetStreams.StreamType.LatLng |
                StravaDotNetStreams.StreamType.Moving |
                StravaDotNetStreams.StreamType.Temp |
                StravaDotNetStreams.StreamType.Time |
                StravaDotNetStreams.StreamType.Velocity_Smooth |
                StravaDotNetStreams.StreamType.Watts,
                StravaDotNetStreams.StreamResolution.All);
            
            ConvertStream(stream);
            _streamSize = stream[0].Data.Count;

            return ;
        }

        private void DownloadLaps()
        {
            LogActivity("Download Laps", _fvActivity);
            foreach (StravaDotNetActivities.ActivityLap stravaLap in _client.Activities.GetActivityLaps(_activityId.ToString()))
            {
                Lap l = Mapper.Map<Lap>(stravaLap);

                // occassionally strava will download a negative moving time which we need to fix.
                if (l.MovingTime < new TimeSpan(0, 0, 0))
                    l.MovingTime = l.ElapsedTime;


                _unitOfWork.CRUDRepository.Add<Lap>(l);
            }
            _unitOfWork.Complete();
        }

        /// <summary>
        /// Convert Strava stream information and write to database.
        /// </summary>
        /// <param name="stream">Detailed information for activity in strava format</param>
        private void ConvertStream(List<StravaDotNetStreams.ActivityStream> stream)
        {
            _convertedStream = ActivityStreams.CreateForNewActivity(this._activityId);

            // convert each strava item
            for (int row = 0; row <= stream[0].OriginalSize - 1; row++)
            {
                Stream s = ExtractStreamRow(stream, row);

                // occasinally we get duplicate rows for the same time back from Strava so need to ignore them!
                if (_convertedStream.Stream.Where(c => c.Time == s.Time).Any())
                {
                    System.Diagnostics.Debug.WriteLine(string.Format("Skipping row {0} for activity {1}", s.Time, _activityId));
                    continue;
                }

                _convertedStream.Stream.Add(s);
            }

            _convertedStream.FixGaps();

            _convertedStream.StoreStreams();
            _convertedStream.CalculatePeaksAndSave();

            // we can only calculate a power curve for activities which have a power meter!
            if (_stravaActivity.HasPowerMeter)
                _convertedStream.AddPowerCurveCalculationJobs();
        }
        
        
        /// <summary>
        /// Convert a single stream row.
        /// </summary>
        /// <param name="activityId">Strava activity id</param>
        /// <param name="stream">Detailed information for activity in strava format</param>
        /// <param name="rowNumber">Row to be processed</param>
        /// <returns></returns>
        private Stream ExtractStreamRow(List<StravaDotNetStreams.ActivityStream> stream, int rowNumber)
        {
            Stream newStream = new Stream();
            newStream.ActivityId = _activityId;

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
                    case StravaDotNetStreams.StreamType.Grade_Smooth: { newStream.Gradient = Convert.ToDouble(s.Data[rowNumber]); break; }
                    case StravaDotNetStreams.StreamType.Heartrate: { newStream.HeartRate = Convert.ToInt32(s.Data[rowNumber]); break; }
                    case StravaDotNetStreams.StreamType.Moving: { newStream.Moving = Convert.ToBoolean(s.Data[rowNumber]); break; }
                    case StravaDotNetStreams.StreamType.Temp: { newStream.Temperature = Convert.ToInt32(s.Data[rowNumber]); break; }
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
        private void ExtractBikeDetails()
        {
            if (!_fvActivity.ActivityType.IsRide)
                return;
        }

        /// <summary>
        /// Download run specific information
        /// </summary>
        /// <param name="activity">StravaActivity</param>
        private void ExtractRunDetails()
        {
            if (!_fvActivity.ActivityType.IsRun)
                return;

            LogActivity("Download Best Efforts", _fvActivity);
            foreach (StravaDotNetActivities.BestEffort effort in _stravaActivity.BestEfforts)
                InsertBestEffort(effort);

              _unitOfWork.Complete();
        }

        /// <summary>
        /// Add best effort information to database.
        /// </summary>
        /// <param name="activityId">Strava activity Id</param>
        /// <param name="e">Strava best effort information</param>
        private void InsertBestEffort(StravaDotNetActivities.BestEffort e)
        {
            BestEffort effort = new BestEffort();

            effort.ActivityId = _activityId;
            effort.MovingTime = TimeSpan.FromSeconds(e.MovingTime);
            effort.Name = e.Name;
            effort.ResourceState = e.ResourceState;
            effort.StartDate = DateTime.Parse(e.StartDate);
            effort.StartDateLocal = DateTime.Parse(e.StartDateLocal);
            effort.StartIndex = e.StartIndex;
            effort.ElapsedTime = TimeSpan.FromSeconds(e.ElapsedTime);
            effort.Distance = Convert.ToDecimal(e.Distance);
            effort.EndIndex = e.EndIndex;
            
            _unitOfWork.CRUDRepository.Add<BestEffort>(effort);
        }
    }
}
