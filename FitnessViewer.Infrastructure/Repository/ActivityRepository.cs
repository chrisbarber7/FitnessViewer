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
                .Include(a=>a.Activity)
                 .Where(s => s.ActivityId == activityId && s.Time % s.Activity.StreamStep == 0)
                 .OrderBy(s => s.Time)
                 .Select(s => new ActivitityCoords
                 {
                     lat = s.Latitude.Value,
                     lng = s.Longitude.Value
                 })

                 .ToList();
        }

       public ActivityGraphStream GetActivityStreams(long activityId)
        {
            IEnumerable<Stream> activityStream =  _context.Stream
                .Include(a=>a.Activity)
                .Where(s => s.ActivityId == activityId && s.Time % s.Activity.StreamStep == 0)
                 .OrderBy(s => s.Time)
                .ToList();

            ActivityGraphStream result = new ActivityGraphStream();

            foreach (Stream s in activityStream)
            {
           
                result.Time.Add(s.Time);
            //    result.Distance.Add(s.Distance);
                result.Altitude.Add(s.Altitude);
          //      result.Velocity.Add(s.Velocity);
                result.HeartRate.Add(s.HeartRate);
                result.Cadence.Add(s.Cadence);
                result.Watts.Add(s.Watts);

            }

            return result;

        }



        public IEnumerable<RunningTimes> GetBestTimes(string userId)
        {
            // temp solution.  Plan is to have a user preferences table which will hold the users favourite distances which will
            // replace this hard coded list.
            List<decimal> favouriteDistances = new List<decimal>()
            {
                805.00M,
                1000.00M,
                1609.00M,
                5000.00M,
                10000.00M,
                21097.00M,
                42195.00M
            };
       
            // get a list of best times
            var times = from t in _context.BestEffort
                        join act in _context.Activity on t.ActivityId equals act.Id
                        join a in _context.Athlete on act.AthleteId equals a.Id
                        join fav in favouriteDistances on t.Distance equals fav
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

        public ActivitySummaryInformation BuildSummaryInformation(long activityId, int startIndex, int endIndex)
        {
            var stream = _context.Stream.Where(s => s.ActivityId == activityId && s.Time >= startIndex && s.Time <= endIndex).ToList();

            if (stream.Count == 0)
                return new ActivitySummaryInformation();

            Stream startDetails = stream.First();//.Where(s => s.Time == startIndex).First();
            Stream endDetails = stream.Last(); // .Where(s => s.Time == endIndex).First();

            // need to group by something to get min/max/ave so grouping by a constant.
            var minMaxAveResults = stream.GroupBy(i => 1)
          .Select(g => new
          {
              powerMin = g.Min(s => s.Watts),
              powerAve = g.Average(s => s.Watts),
              powerMax = g.Max(s => s.Watts),
              heartRateMin = g.Min(s => s.HeartRate),
              heartRateAve = g.Average(s => s.HeartRate),
              heartRateMax = g.Max(s => s.HeartRate),
              cadenceMin = g.Min(s => s.Cadence),
              cadenceAve = g.Average(s => s.Cadence),
              cadenceMax = g.Max(s => s.Cadence),
              elevationMin = g.Min(s => s.Altitude),
              elevationAve = g.Average(s => s.Altitude),
              elevationMax = g.Max(s => s.Altitude),
              time = g.Count(),
              distance = g.Sum(s => s.Distance)
          }).First();

            ActivitySummaryInformation info = new ActivitySummaryInformation();
            if (minMaxAveResults.powerMin == null)
            {
                info.Power = null;
            }
            else
            {
                info.Power.Min = minMaxAveResults.powerMin.Value;
                info.Power.Max = minMaxAveResults.powerMax.Value;
                info.Power.Ave = Convert.ToInt32(minMaxAveResults.powerAve.Value);
            }

            if (minMaxAveResults.heartRateMin == null)
            {
                info.HeartRate = null;
            }
            else
            {
                info.HeartRate.Min = minMaxAveResults.heartRateMin.Value;
                info.HeartRate.Max = minMaxAveResults.heartRateMax.Value;
                info.HeartRate.Ave = Convert.ToInt32(minMaxAveResults.heartRateAve.Value);
            }

            if (minMaxAveResults.cadenceMin == null)
            {
                info.Cadence = null;
            }
            else
            {
                info.Cadence.Min = minMaxAveResults.cadenceMin.Value;
                info.Cadence.Max = minMaxAveResults.cadenceMax.Value;
                info.Cadence.Ave = Convert.ToInt32(minMaxAveResults.cadenceAve.Value);
            }

            if (minMaxAveResults.elevationMin == null) {
                info.Elevation = null;
            }
            else
            {

                info.Elevation.Min = Convert.ToInt32(minMaxAveResults.elevationMin.Value);
                info.Elevation.Ave = Convert.ToInt32(minMaxAveResults.elevationAve.Value);
                info.Elevation.Max = Convert.ToInt32(minMaxAveResults.elevationMax.Value);
            }

            if (minMaxAveResults.distance != null)
                info.Distance = MetreDistance.ToMiles(Convert.ToDecimal(endDetails.Distance.Value - startDetails.Distance.Value));

            info.Time = TimeSpan.FromSeconds(endIndex - startIndex);

            return info;

        }

        public ActivityPeakDetail GetActivityPeakDetail(int id)
        {
            return _context.ActivityPeakDetail.Where(p => p.Id == id).FirstOrDefault();
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
                    TotalDistance = Math.Round(r.Sum(d => d.DistanceInMiles), 1),
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
            var result = _context.Lap
                .Include(a => a.Activity)
                .Where(l => l.ActivityId == activityId).OrderBy(l => l.LapIndex)
                  .Select(l => new ActivityLap
                  {
                      Id = l.Id,
                      Type = PeakStreamType.Lap,
                      Selected = false,
                      Name = l.Name,
                      Value = l.ElapsedTime.ToString(),
                      StartIndex = l.StartIndex / l.Activity.StreamStep,
                      EndIndex = l.EndIndex / l.Activity.StreamStep,
                      StreamStep = l.Activity.StreamStep
                  });


            return result.ToList();
        }


        public Lap GetLap(int id)
        {
            return _context.Lap.Where(l => l.Id == id).FirstOrDefault();
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
                  Value = p.Value.ToString(),
                  StartIndex = p.StartIndex.Value / p.Activity.StreamStep,
                  EndIndex = p.EndIndex.Value / p.Activity.StreamStep,
                  StreamStep = p.Activity.StreamStep
              }).ToList();


            foreach (ActivityLap l in result)
            {
                l.Name = StreamHelper.StreamDurationForDisplay(Convert.ToInt32(l.Name));
            }
            return result;
        }
    }
}

