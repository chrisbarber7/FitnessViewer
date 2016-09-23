using Strava.Athletes;
using Strava.Authentication;
using Strava.Clients;
using System;
using FitnessViewer.Infrastructure.Data;
using FitnessViewer.Infrastructure.Models;
using Strava.Activities;
using com.strava.api.Activities;
using System.Collections.Generic;
using Strava.Streams;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Linq;
using Strava.Api;

namespace FitnessViewer.Infrastructure.Helpers
{
    /// <summary>
    /// Used to get data from strava and add/update the information in the database
    /// </summary>
    public class Strava
    {
        private Repository _repo;
        private StravaClient _client;
        private string _userId;
        private long _stravaId;

        public Strava()
        {
            _repo = new Repository();
            Limits.UsageChanged += Limits_UsageChanged;
        }

        private void Limits_UsageChanged(object sender, UsageChangedEventArgs e)
        {
            // if getting close to short term limit introduce delays.
            if (e.Usage.ShortTerm < 550)
                return;

            System.Diagnostics.Debug.WriteLine("Short: "+e.Usage.ShortTerm.ToString());
            System.Diagnostics.Debug.WriteLine("Long : " + e.Usage.LongTerm.ToString());
            System.Threading.Thread.Sleep(30000);

        }


        /// <summary>
        /// Constructor for a given identity user id (token looked up)
        /// </summary>
        /// <param name="userId">Identity userid</param>
        public Strava(string userId)
        {
            _repo = new Repository();
            _userId = userId;
            Limits.UsageChanged += Limits_UsageChanged;

            string token = _repo.FindAthleteByUserId(userId).Token;

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
            _repo = new Repository();
            this._stravaId = stravaAthleteId;
            SetupClient(token);
        }

        /// <summary>
        /// Create Strava client used for downloading information
        /// </summary>
        /// <param name="token">Strava Access Token</param>
        private void SetupClient(string token)
        {
            StaticAuthentication auth = new StaticAuthentication(token);
            _client = new StravaClient(auth);
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
            Athlete athlete = _client.Athletes.GetAthlete();
            var a = new StravaAthlete();
            a.Id = athlete.Id;
            a.UserId = userId;
            a.Token = token;
            UpdateEntityWithStravaDetails(athlete, a);
            _repo.AddAthlete(a);

            // add user to the strava download queue for background downloading of activities.
            _repo.AddQueueItem(userId);
        }    

        /// <summary>
        /// Update strava athlete details
        /// </summary>
        /// <param name="token">Strava access token</param>
        public void UpdateAthlete(string token)
        {
            Athlete stravaAthleteDetails = _client.Athletes.GetAthlete();
            StravaAthlete a = _repo.FindAthleteById(this._stravaId);

            if (a == null)
                return;

            a.Id = stravaAthleteDetails.Id;            
            a.Token = token;
            UpdateEntityWithStravaDetails(stravaAthleteDetails, a);
            _repo.EditAthlete(a);
        }

        /// <summary>
        /// Download all activities for athlete from strava and write to database.
        /// </summary>
        public void AddActivitesForAthlete()
        {
            StravaAthlete a = _repo.FindAthleteByUserId(_userId);

            if (a == null)
                throw new ArgumentException("Invalid UserId");

            // max activities allowed by strava in each download.
            const int perPage = 200;

            int page = 1;

            // loop until no activities are downloaded in last request to strava.
            while (true)
            {
                var activities = _client.Activities.GetActivities(new DateTime(2016, 1, 1), DateTime.Now, page++, perPage);

                if (activities.Count == 0)
                    break;

                List<StravaActivity> newActivities = new List<StravaActivity>();

                foreach (var item in activities)
                {
                    StravaActivity activity = InsertActivity(a.Id, item);

                    if (activity == null)
                        continue;

                    newActivities.Add(activity);

                    // put the new activity in the queue so that we'll download the full activity details.
                    _repo.AddQueueItem(a.UserId, activity.Id);
                }
                _repo.AddActivity(newActivities);
                
                // write changes to database.
                _repo.SaveChanges();
            }
        }
        
        /// <summary>
        /// Download detailed information for given activity
        /// </summary>
        /// <param name="activityId">Strava activity Id</param>
        public void ActivityDetailsDownload(long activityId)
        {
            StravaActivity activity = _repo.GetActivity(activityId);

            System.Diagnostics.Debug.WriteLine(string.Format("{0} {1}", activity.StartDateLocal.ToShortDateString(),
                activity.Name));

            // heart rate/power zones
            //List<ActivityZone> zones = _client.Activities.GetActivityZones(activity.Id.ToString());

            // detailed information
            List<ActivityStream> stream = _client.Streams.GetActivityStream(activity.Id.ToString(),
                StreamType.Altitude |
                StreamType.Cadence |
                StreamType.Distance |
                StreamType.GradeSmooth |
                StreamType.Heartrate |
                StreamType.LatLng |
                StreamType.Moving |
                StreamType.Temperature |
                StreamType.Time |
                StreamType.Velocity_Smooth |
                StreamType.Watts,
                StreamResolution.All);

            ExtractAndStoreStream(activity.Id, stream);

            if (activity.Type == "Run")
                RunDetailsDownload(activity);
            else if (activity.Type == "Ride" || activity.Type == "VirtualRide")
                BikeDetailsDownload(activity);

        }

        /// <summary>
        /// Convert Strava stream information and write to database.
        /// </summary>
        /// <param name="activityId">Strava activity id</param>
        /// <param name="stream">Detailed information for activity in strava format</param>
        private void ExtractAndStoreStream(long activityId, List<ActivityStream> stream)
        {
            List<StravaStream> convertedStream = new List<StravaStream>();

            // convert each strava item
            for (int row = 0; row <= stream[0].OriginalSize - 1; row++)
                convertedStream.Add(ExtractStreamRow(activityId, stream, row));

            ExtractPeaksFromStream(activityId, convertedStream.Select(s => s.Watts).ToList(), PeakStreamType.Power);
            ExtractPeaksFromStream(activityId, convertedStream.Select(s => s.Cadence).ToList(), PeakStreamType.Cadence);
            ExtractPeaksFromStream(activityId, convertedStream.Select(s => s.HeartRate).ToList(), PeakStreamType.HeartRate);

            // write all details to database.
            _repo.AddSteam(convertedStream);
        }

        private void ExtractPeaksFromStream(long activityId, List<int?> stream, PeakStreamType type)
        {
            if (!stream.Contains(null))
            {
                PeakValueFinder finder = new PeakValueFinder(
                    stream.Select(s => s.Value).ToList(),
                    PeakStreamType.Power);

                List<PeakDetail> peaks = finder.FindPeaks();

                _repo.AddPeak(activityId, type, peaks);
            }
        }

        /// <summary>
        /// Convert a single stream row.
        /// </summary>
        /// <param name="activityId">Strava activity id</param>
        /// <param name="stream">Detailed information for activity in strava format</param>
        /// <param name="rowNumber">Row to be processed</param>
        /// <returns></returns>
        private static StravaStream ExtractStreamRow(long activityId, List<ActivityStream> stream, int rowNumber)
        {
            StravaStream newStream = new StravaStream();
            newStream.StravaActivityId = activityId;

            foreach (ActivityStream s in stream)
            {
                switch (s.StreamType)
                {
                    case StreamType.LatLng:
                        {
                            // convert json lat/long  
                            JArray o = (JArray)s.Data[rowNumber];
                            newStream.Latitude = (double)o.First;
                            newStream.Longitude = (double)o.Last;

                            break;
                        }
                    case StreamType.Altitude: { newStream.Altitude = Convert.ToDouble(s.Data[rowNumber]); break; }
                    case StreamType.Cadence: { newStream.Cadence = Convert.ToInt32(s.Data[rowNumber]); break; }
                    case StreamType.Distance: { newStream.Distance = Convert.ToDouble(s.Data[rowNumber]); break; }
                    case StreamType.GradeSmooth: { newStream.Gradient = Convert.ToDouble(s.Data[rowNumber]); break; }
                    case StreamType.Heartrate: { newStream.HeartRate = Convert.ToInt32(s.Data[rowNumber]); break; }
                    case StreamType.Moving: { newStream.Moving = Convert.ToBoolean(s.Data[rowNumber]); break; }
                    case StreamType.Temperature: { newStream.Temperature = Convert.ToInt32(s.Data[rowNumber]); break; }
                    case StreamType.Time: { newStream.Time = Convert.ToInt32(s.Data[rowNumber]); break; }
                    case StreamType.Velocity_Smooth: { newStream.Velocity = Convert.ToDouble(s.Data[rowNumber]); break; }
                    case StreamType.Watts: { newStream.Watts = Convert.ToInt32(s.Data[rowNumber]); break; }
                }
            }

            return newStream;
        }

        /// <summary>
        /// Download bike specific information
        /// </summary>
        /// <param name="activity">StravaActivity</param>
        private void BikeDetailsDownload(StravaActivity activity)
        {
        }

        /// <summary>
        /// Download run specific information
        /// </summary>
        /// <param name="activity">StravaActivity</param>
        private void RunDetailsDownload(StravaActivity activity)
        {
            var act = _client.Activities.GetActivity(activity.Id.ToString(), true);

            foreach (BestEffort effort in act.BestEfforts)
                InsertBestEffort(activity.Id, effort);

            _repo.SaveChanges();
        }

        /// <summary>
        /// Add best effort information to database.
        /// </summary>
        /// <param name="activityId">Strava activity Id</param>
        /// <param name="e">Strava best effort information</param>
        private void InsertBestEffort(long activityId, BestEffort e)
        {
            StravaBestEffort effort = new StravaBestEffort();

            effort.StravaActivityId = activityId;
            effort.MovingTime = TimeSpan.FromSeconds(e.MovingTime);
            effort.Name = e.Name;
            effort.ResourceState = e.ResourceState;
            effort.StartDate = DateTime.Parse(e.StartDate);
            effort.StartDateLocal = DateTime.Parse(e.StartDateLocal);
            effort.StartIndex = e.StartIndex;
            effort.ElapsedTime = TimeSpan.FromSeconds(e.ElapsedTime);
            effort.Distance = e.Distance;
            effort.EndIndex = e.EndIndex;

            _repo.AddBestEffort(effort);            
        }

        /// <summary>
        /// Add strava activity to database.
        /// </summary>
        /// <param name="athleteId">Strava Athlete Id</param>
        /// <param name="item">Strava Summary infomation</param>
        /// <returns></returns>
        private StravaActivity InsertActivity(long athleteId, ActivitySummary item)
        {
            StravaActivity s = new StravaActivity();

            try
            {
                s.StravaAthleteId = athleteId;
                s.Id = item.Id;
                s.Name = item.Name;
                s.ExternalId = item.ExternalId;
                s.Type = item.Type.ToString();
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
            }
            catch (Exception ex)
            {
                // catches event types (i.e. watersport) which aren't handled.  Ignoring these activities for now
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return null;
            }

            return s;
        }

        /// <summary>
        /// Map strava fields on strava/FV.
        /// </summary>
        /// <param name="athlete">Strava athlete details</param>
        /// <param name="a">FV Athlete details</param>
        private static void UpdateEntityWithStravaDetails(Athlete athlete, StravaAthlete a)
        {
            a.FirstName = athlete.FirstName;
            a.LastName = athlete.LastName;
            a.ProfileMedium = athlete.ProfileMedium;
            a.Profile = athlete.Profile;
            a.City = athlete.City;
            a.State = athlete.State;
            a.Country = athlete.Country;
            a.Sex = athlete.Sex;
            a.Friend = athlete.Friend;
            a.Follower = athlete.Follower;
            a.IsPremium = athlete.IsPremium;
            a.CreatedAt = athlete.CreatedAt;
            a.UpdatedAt = athlete.UpdatedAt;
            a.ApproveFollowers = athlete.ApproveFollowers;
            a.AthleteType = athlete.AthleteType;
            a.DatePreference = athlete.DatePreference;
            a.MeasurementPreference = athlete.MeasurementPreference;
            a.Email = athlete.Email;
            a.FTP = athlete.Ftp;
            a.Weight = athlete.Weight;
        }
    }
}