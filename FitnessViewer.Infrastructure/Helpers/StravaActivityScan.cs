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
using FitnessViewer.Infrastructure.Data;

namespace FitnessViewer.Infrastructure.Helpers
{
    public class StravaActivityScan : Strava
    {
        public StravaActivityScan(UnitOfWork uow, string userId) : base(uow, userId)
        { }

        /// <summary>
        /// Download all activities for athlete from strava and write to database.
        /// </summary>
        public void AddActivitesForAthlete()
        {
            Athlete fvAthlete = _unitOfWork.Athlete.FindAthleteByUserId(_userId);

            if (fvAthlete == null)
                throw new ArgumentException("Invalid UserId");

            LogActivity("Add Activities", fvAthlete);

            // max activities allowed by strava in each download.
            const int perPage = 200;

            int page = 1;
            int itemsAdded = 0;

            // default to only looking for the last 90 days activities.
            DateTime startDate = DateTime.Now.AddDays(-90);

            List<long> currentActivities = _unitOfWork.Activity.GetActivities(_userId).Select(a => a.Id ).ToList();

            // if no activites then we're doing a full download (or the last 5 years worth).
            if (currentActivities.Count == 0)
                startDate = DateTime.Now.AddYears(-5);

            // loop until no activities are downloaded in last request to strava.
            while (true)
            {
                var activities = _client.Activities.GetActivities(startDate, DateTime.Now, page++, perPage);

                if (activities.Count == 0)
                    break;

                List<DownloadQueue> jobs = new List<DownloadQueue>();

                foreach (var item in activities)
                {
                    // if activity already exists skip it
                    if (currentActivities.Contains(item.Id))
                        continue;

                    Models.Activity activity = InsertActivity(fvAthlete.Id, item);

                    if (activity == null)
                        continue;

                    fvAthlete.Activities.Add(activity);

                    // put the new activity in the queue so that we'll download the full activity details.
                    DownloadQueue job = DownloadQueue.CreateQueueJob(fvAthlete.UserId, DownloadType.Strava, item.Id);
                    _unitOfWork.Queue.AddQueueItem(job);
                    jobs.Add(job);
                    
          
                    itemsAdded++;
                }

                // write changes to database.
                _unitOfWork.Complete();


                foreach (DownloadQueue job in jobs)
                    job.AddToAzureQueue();
       
                if (stravaLimitDelay > 100)
                    LogActivity(string.Format("Pausing for {0}ms", stravaLimitDelay.ToString()), fvAthlete);

                System.Threading.Thread.Sleep(stravaLimitDelay);
            }

            if (itemsAdded > 0)
            {
                // add a notification 
                _unitOfWork.Notification.Add(new UserNotification(_userId, Notification.StravaActivityScan(itemsAdded)));
                _unitOfWork.Complete();

                // trigger web job to download activity details.
                AzureWebJob.CreateTrigger(_unitOfWork);
            }
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
                s.Distance = Convert.ToDecimal(item.Distance);
                s.TotalPhotoCount = item.TotalPhotoCount;
                s.ElevationGain = Convert.ToDecimal(item.ElevationGain);
                s.HasKudoed = item.HasKudoed;
                s.AverageHeartrate = Convert.ToDecimal(item.AverageHeartrate);
                s.MaxHeartrate = Convert.ToDecimal(item.MaxHeartrate);
                s.Truncated = item.Truncated;
                s.GearId = item.GearId;
                s.AverageSpeed = Convert.ToDecimal(item.AverageSpeed);
                s.MaxSpeed = Convert.ToDecimal(item.MaxSpeed);
                s.AverageCadence = Convert.ToDecimal(item.AverageCadence);
                s.AverageTemperature = Convert.ToDecimal(item.AverageTemperature);
                s.AveragePower = Convert.ToDecimal(item.AveragePower);
                s.Kilojoules = Convert.ToDecimal(item.Kilojoules);
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

                s.DetailsDownloaded = false;
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
