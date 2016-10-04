﻿using System;
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
    public class StravaActivityScan : Strava
    {
        public StravaActivityScan(string userId) : base(userId)
        { }

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