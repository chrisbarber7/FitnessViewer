using FitnessViewer.Infrastructure.Data;
using FitnessViewer.Infrastructure.Models;
using FitnessViewer.Infrastructure.Models.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using FitnessViewer.Infrastructure.enums;
using FitnessViewer.Infrastructure.Helpers;

namespace FitnessViewer.Infrastructure.Repository
{
    public class ActivityRepository
    {
        private ApplicationDb _context;


        public ActivityRepository(ApplicationDb context)
        {
            _context = context;
        }

        #region activity
        public void AddActivity(Activity a)
        {
            _context.Activity.Add(a);

        }

        public void AddActivity(IEnumerable<Activity> activities)
        {
            _context.Activity.AddRange(activities);
        }

        public Activity GetActivity(long activityId)
        {
            return _context.Activity.Where(a => a.Id == activityId)
                .Include(a => a.ActivityType)
                .FirstOrDefault();
        }

        internal IQueryable<Stream> GetStream()
        {
            return _context.Stream;
        }



        internal IQueryable<Stream> GetStreamForActivity(long activityId)
        {
            return _context.Stream.Where(s => s.ActivityId == activityId);
        }


        public IEnumerable<Activity> GetActivities(string userId)
        {
            return _context.Activity
                  .Where(a => a.Athlete.UserId == userId)
                  .Include(a => a.ActivityType)
                  .ToList();

        }

        #endregion

        #region Best Effort
        public void AddBestEffort(BestEffort e)
        {
            _context.BestEffort.Add(e);
        }
        #endregion

        #region streams
        public void AddStream(IEnumerable<Stream> s)
        {
            _context.Stream.AddRange(s);
        }

        #endregion

        public void AddOrUpdateGear(Gear g)
        {
            _context.Gear.AddOrUpdate(g);
        }

        public IEnumerable<ActivitityCoords> GetActivityCoords(long activityId)
        {
            return _context.Stream
                .Where(s => s.ActivityId == activityId)
                .Select(s => new ActivitityCoords
                {
                    lat = s.Latitude.Value,
                    lng = s.Longitude.Value
                })
                .ToList();
        }

        public IEnumerable<RunningTimes> GetBestTimes(string userId)
        {
            // get a list of best times
            var times = from t in _context.BestEffort
                        join act in _context.Activity on t.ActivityId equals act.Id
                        join a in _context.Athlete on act.AthleteId equals a.Id
                        where a.UserId == userId
                        group t by t.Name into dptgrp
                        let fastestTime = dptgrp.Min(x => x.ElapsedTime)
                        select new
                        {
                            DistanceName = dptgrp.Key,
                            BestEffortId = dptgrp.FirstOrDefault(y => y.ElapsedTime == fastestTime).Id,
                            Time = fastestTime

                        };

            // join to other table to get full info.
            var results = from t in times
                          join e in _context.BestEffort on t.BestEffortId equals e.Id
                          join a in _context.Activity on e.ActivityId equals a.Id
                          orderby t.Time
                          select new RunningTimes
                          {
                              ActivityName = a.Name,
                              ActivityDate = a.StartDateLocal,
                              DistanceName = t.DistanceName,
                              Distance = e.Distance,
                              Time = t.Time,
                              ActivityId = e.ActivityId
                          };

            return results.ToList();
        }

        public IEnumerable<ActivityByPeriod> ActivityByWeek(string userId, string activityType, DateTime start, DateTime end)
        {
            return _context.Activity
                .Include(r => r.Calendar)
                .Include(r => r.ActivityType)
                .Include(r => r.Athlete)
                .Where(r => 
                        r.Athlete.UserId == userId &&
                        r.Start >= start &&
                        r.Start <= end &&
                        (r.ActivityTypeId == activityType || activityType == "All"))
                .GroupBy(r => new { ActivityType = r.ActivityType.Description, YearWeek = r.Calendar.YearWeek, Label = r.Calendar.WeekLabel })
                .Select(r => new ActivityByPeriod
                {
                    ActivityType = r.Key.ActivityType,
                    Period = r.Key.YearWeek,
                    TotalDistance = (float)Math.Round(r.Sum(d => d.DistanceInMiles), 1),
                    Number = r.Select(i => i.Id).Distinct().Count(),
                    Label = r.Key.Label
                })
                .OrderBy(r => r.Period)
                .ToList();
        }

        internal void AddLap(Lap lap)
        {
            _context.Lap.Add(lap);     
        }


        public IEnumerable<ActivityLap> GetLaps(long activityId)
        {
          var result=  _context.Lap.Where(l => l.ActivityId == activityId).OrderBy(l => l.LapIndex)
                .Select(l => new ActivityLap {
                    Id = l.Id,
                    Type = PeakStreamType.Lap,
                    Selected = false,
                    Name = l.Name,
                    Value = l.ElapsedTime.ToString()
                });


            return result.ToList();
        }

        public IEnumerable<ActivityLap> GetLapStream(long activityId, PeakStreamType streamType)
        {
            string units = StreamHelper.StreamTypeUnits(streamType);

            var result = _context.ActivityPeakDetail
              .Where(p => p.ActivityId == activityId && p.StreamType == streamType)
              .OrderBy(p => p.Duration)
              .Select(p => new ActivityLap
              {
                  Id = p.Id,
                  Type = streamType,
                  Selected = false,
                  Name = p.Duration.ToString(),
                  Value = p.Value.ToString()
              }).ToList();


            foreach (ActivityLap l in result)
            {
                l.Name = StreamHelper.StreamDurationForDisplay(Convert.ToInt32(l.Name));
            }
            return result;
        }
    }
}

