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
    /// <summary>
    /// Used to get data from strava and add/update the information in the database
    /// </summary>
    public class Strava
    {
        private Data.UnitOfWork _unitOfWork;
        private StravaDotNetClient.StravaClient _client;
        private string _userId;
        private long _stravaId;
        private int stravaLimitDelay;

        public Strava()
        {
            _unitOfWork = new Data.UnitOfWork();
            StravaDotNetApi.Limits.UsageChanged += Limits_UsageChanged;
        }

        private void Limits_UsageChanged(object sender, StravaDotNetApi.UsageChangedEventArgs e)
        {
             // if getting close to short term limit introduce delays.
            if (e.Usage.ShortTerm < 550)
                stravaLimitDelay = 100;
            else if (e.Usage.ShortTerm <=575)
                stravaLimitDelay = 30000;
            else if (e.Usage.ShortTerm <= 590)
                stravaLimitDelay = 45000;
            else
                stravaLimitDelay = 60000;
        }
        
        /// <summary>
        /// Constructor for a given identity user id (token looked up)
        /// </summary>
        /// <param name="userId">Identity userid</param>
        public Strava(string userId)
        {
            _unitOfWork = new Data.UnitOfWork();
            _userId = userId;
            StravaDotNetApi.Limits.UsageChanged += Limits_UsageChanged;

            string token = _unitOfWork.Athlete.FindAthleteByUserId(userId).Token;

            if (string.IsNullOrEmpty(token))
                throw new ArgumentException("Invalid UserId");

            SetupClient(token);
        }

        /// <summary>
        /// Constructor for creating a new athlete.
        /// </summary>
        /// <param name="stravaAthleteId">Strava athlete id</param>
        /// <param name="token">Strava access token</param>
        public Strava(long stravaAthleteId, string token)
        {
            _unitOfWork = new Data.UnitOfWork();
            this._stravaId = stravaAthleteId;
            SetupClient(token);
        }

        /// <summary>
        /// Create Strava client used for downloading information
        /// </summary>
        /// <param name="token">Strava Access Token</param>
        private void SetupClient(string token)
        {
            StravaDotNetAuthentication.StaticAuthentication auth = new StravaDotNetAuthentication.StaticAuthentication(token);
            _client = new StravaDotNetClient.StravaClient(auth);
        }

        /// <summary>
        /// Add strava athlete details to data
        /// </summary>
        /// <param name="userId">Indentity userId</param>
        /// <param name="token">Strava access token</param>
        public void AddAthlete(string userId, string token)
        {
            _userId = userId;
            SetupClient(token);
            StravaDotNetAthletes.Athlete athlete = _client.Athletes.GetAthlete();

            Athlete a = Mapper.Map<Athlete>(athlete);
            a.UserId = userId;
            a.Token = token;

            _unitOfWork.Athlete.AddAthlete(a);

            UpdateBikes(a.Id, athlete.Bikes);
            UpdateShoes(a.Id, athlete.Shoes);

            // add user to the strava download queue for background downloading of activities.
            _unitOfWork.Queue.AddQueueItem(userId);

            _unitOfWork.Complete();
        }

        /// <summary>
        /// Update strava athlete details
        /// </summary>
        /// <param name="token">Strava access token</param>
        public void UpdateAthlete(string token)
        {
            StravaDotNetAthletes.Athlete stravaAthleteDetails = _client.Athletes.GetAthlete();
            Athlete fitnessViewerAthlete = _unitOfWork.Athlete.FindAthleteById(this._stravaId);

            if (fitnessViewerAthlete == null)
                return;

            LogActivity("Updating Athlete", fitnessViewerAthlete);


            Mapper.Map(stravaAthleteDetails, fitnessViewerAthlete);
                   
            fitnessViewerAthlete.Token = token;

            UpdateBikes(stravaAthleteDetails.Id, stravaAthleteDetails.Bikes);
            UpdateShoes(stravaAthleteDetails.Id, stravaAthleteDetails.Shoes);

            _unitOfWork.Complete();
        }

        private void LogActivity(string action, Athlete fitnessViewerAthlete)
        {

            string log = string.Format("{0} : {1} ({2} {3}) ", action,
                                                               fitnessViewerAthlete.Id,
                                                               fitnessViewerAthlete.FirstName,
                                                               fitnessViewerAthlete.LastName);

            System.Diagnostics.Debug.WriteLine(log);
            Console.WriteLine(log);
        }

        private void LogActivity(string action, Activity activity )
        {

            string log = string.Format("{0} : {1} - {2}", action,
                                                          activity.Id,
                                                          activity.Name);
                                                                                
            System.Diagnostics.Debug.WriteLine(log);
            Console.WriteLine(log);
        }

        private void UpdateBikes(long athleteId, List<StravaDotNetGear.Bike> bikes)
        {
            foreach (StravaDotNetGear.Bike b in bikes)
            {
                Gear g = Gear.CreateBike(b.Id, athleteId);

                g.Brand = b.Brand;
                g.Description = b.Description;
                g.Distance = b.Distance;
                switch (b.FrameType)
                {
                    case StravaDotNetGear.BikeType.Cross: { g.FrameType = enums.BikeType.Cross; break; }
                    case StravaDotNetGear.BikeType.Mountain: { g.FrameType = enums.BikeType.Mountain; break; }
                    case StravaDotNetGear.BikeType.Road: { g.FrameType = enums.BikeType.Road; break; }
                    case StravaDotNetGear.BikeType.Timetrial: { g.FrameType = enums.BikeType.Timetrial; break; }
                }
                g.IsPrimary = b.IsPrimary;
                g.Model = b.Model;
                g.Name = b.Name;
                g.ResourceState = b.ResourceState;

                _unitOfWork.Activity.AddOrUpdateGear(g);
            }
        }


        private void UpdateShoes(long athleteId, List<StravaDotNetGear.Shoes> shoes)
        { 
            foreach (StravaDotNetGear.Shoes s in shoes)
            {
                Gear g = Gear.CreateShoe(s.Id, athleteId);
                g.Distance = s.Distance;
                g.FrameType = enums.BikeType.Default;
                g.IsPrimary = s.IsPrimary;
                g.Name = s.Name;
                g.ResourceState = s.ResourceState;
                _unitOfWork.Activity.AddOrUpdateGear(g);
            }
        }

        /// <summary>
        /// Download all activities for athlete from strava and write to database.
        /// </summary>
        public void AddActivitesForAthlete()
        {
            Athlete a = _unitOfWork.Athlete.FindAthleteByUserId(_userId);

            if (a == null)
                throw new ArgumentException("Invalid UserId");

            LogActivity("Add Activities", a);

            // max activities allowed by strava in each download.
            const int perPage = 200;

            int page = 1;

            // loop until no activities are downloaded in last request to strava.
            while (true)
            {
                var activities = _client.Activities.GetActivities(DateTime.Now.AddDays(-7), DateTime.Now, page++, perPage);

                if (activities.Count == 0)
                    break;

                foreach (var item in activities)
                {
                    // if activity already exists skip it
                    if (_unitOfWork.Activity.GetActivities(_userId).Any(act => act.Id == item.Id))
                        continue;

                    Models.Activity activity = InsertActivity(a.Id, item);

                    if (activity == null)
                        continue;

                    a.Activities.Add(activity);

                    // put the new activity in the queue so that we'll download the full activity details.
                    _unitOfWork.Queue.AddQueueItem(a.UserId, item.Id);
                }

                // write changes to database.
                _unitOfWork.Complete();

                if (stravaLimitDelay > 100)
                    LogActivity(string.Format("Pausing for {0}ms", stravaLimitDelay.ToString()), a);

                System.Threading.Thread.Sleep(stravaLimitDelay);
            }
        }
        
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
            fvActivity.Calories = stravaActivity.Calories;
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
                convertedStream.Add(ExtractStreamRow(activityId, stream, row));

            ExtractPeaksFromStream(activityId, convertedStream.Select(s => s.Watts).ToList(), PeakStreamType.Power);
            ExtractPeaksFromStream(activityId, convertedStream.Select(s => s.Cadence).ToList(), PeakStreamType.Cadence);
            ExtractPeaksFromStream(activityId, convertedStream.Select(s => s.HeartRate).ToList(), PeakStreamType.HeartRate);

            // write all details to database.
            _unitOfWork.Activity.AddStream(convertedStream);
        }

        public void ExtractPeaksFromStream(long activityId, List<int?> stream, PeakStreamType type)
        {
            if (!stream.Contains(null))
            {
                PeakValueFinder finder = new PeakValueFinder(
                    stream.Select(s => s.Value).ToList(),
                    PeakStreamType.Power,
                    activityId);

                List<ActivityPeakDetail> peaks = finder.FindPeaks();

                _unitOfWork.Analysis.AddPeak(activityId, type, peaks);
            }
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
            effort.Distance = e.Distance;
            effort.EndIndex = e.EndIndex;         

          _unitOfWork.Activity.AddBestEffort(effort);            
        }

        /// <summary>
        /// Add strava activity to database.
        /// </summary>
        /// <param name="athleteId">Strava Athlete Id</param>
        /// <param name="item">Strava Summary infomation</param>
        /// <returns></returns>
        private Activity InsertActivity(long athleteId, StravaDotNetActivities.ActivitySummary item)
        {
            Models.Activity s = new Models.Activity();

            try
            {
                s.AthleteId = athleteId;
                s.Id = item.Id;
                s.Name = item.Name;
                s.ExternalId = item.ExternalId;
                s.ActivityTypeId = item.Type.ToString();
                s.SufferScore = item.SufferScore;
                s.EmbedToken = item.EmbedToken;
                s.Distance = item.Distance;
                s.TotalPhotoCount = item.TotalPhotoCount;
                s.ElevationGain = item.ElevationGain;
                s.HasKudoed = item.HasKudoed;
                s.AverageHeartrate = item.AverageHeartrate;
                s.MaxHeartrate = item.MaxHeartrate;
                s.Truncated = item.Truncated;
                s.GearId = item.GearId;
                s.AverageSpeed = item.AverageSpeed;
                s.MaxSpeed = item.MaxSpeed;
                s.AverageCadence = item.AverageCadence;
                s.AverageTemperature = item.AverageTemperature;
                s.AveragePower = item.AveragePower;
                s.Kilojoules = item.Kilojoules;
                s.IsTrainer = item.IsTrainer;
                s.IsCommute = item.IsCommute;
                s.IsManual = item.IsManual;
                s.IsPrivate = item.IsPrivate;
                s.IsFlagged = item.IsFlagged;
                s.AchievementCount = item.AchievementCount;
                s.KudosCount = item.KudosCount;
                s.CommentCount = item.CommentCount;
                s.AthleteCount = item.AthleteCount;
                s.PhotoCount = item.PhotoCount;
                s.StartDate = item.DateTimeStart;
                s.StartDateLocal = item.DateTimeStartLocal;
                s.MovingTime = item.MovingTimeSpan;

                if (item.ElapsedTimeSpan.Days >= 1)
                {
                    if (s.MovingTime.Value.Days == 0)
                        s.MovingTime = item.MovingTimeSpan;
                    else
                        s.MovingTime = null;
                }
                else
                    s.ElapsedTime = item.ElapsedTimeSpan;

                s.TimeZone = item.TimeZone;
                s.StartLatitude = item.StartLatitude;
                s.StartLongitude = item.StartLongitude;
                s.WeightedAverageWatts = item.WeightedAverageWatts;

                s.EndLatitude = item.EndLatitude;
                s.EndLongitude = item.EndLongitude;
                s.HasPowerMeter = item.HasPowerMeter;

                s.MapId = item.Map.Id;
                s.MapPolyline = item.Map.Polyline;
                s.MapPolylineSummary = item.Map.SummaryPolyline;
            }
            catch (Exception ex)
            {
                // catches event types (i.e. watersport) which aren't handled.  Ignoring these activities for now
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return null;
            }

            return s;
        }    
    }
}