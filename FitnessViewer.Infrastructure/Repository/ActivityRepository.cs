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
using EntityFramework.BulkInsert.Extensions;
using System.Data.SqlClient;

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
        //public void AddActivity(Activity a)
        //{
        //    _context.Activity.Add(a);

        //}

        //public void AddActivity(IEnumerable<Activity> activities)
        //{
        //    _context.Activity.AddRange(activities);
        //}


        public Activity GetActivity(long activityId)
        {

            return _context.Activity.Where(a => a.Id == activityId)
                .Include(a => a.ActivityType)
                .Include(a=> a.Athlete)
                .FirstOrDefault();
        }



        
        //public IEnumerable<ActivityPeakDetail> GetActivityPeakDetails(long id, PeakStreamType streamType)
        //{

        //    return _context.ActivityPeakDetail.Where(a => a.ActivityId == id && a.StreamType == streamType).ToList();
        //}

        public IEnumerable<ActivityPeakDetail> GetActivityPeakDetails(long id)
        {
            return _context.ActivityPeakDetail.Where(a => a.ActivityId == id )
                .OrderBy(a=>a.StreamType)
                .ThenBy(a=>a.Duration)
                .ToList();
        }


        //public IEnumerable<ActivityPeaks> GetActivityPeaks(long id)
        //{
        //    return _context.ActivityPeak.Where(p => p.ActivityId == id).ToList();
        //}

        //internal IQueryable<Stream> GetStream()
        //{
        //    return _context.Stream;
        //}



        internal IQueryable<Stream> GetStreamForActivity(long activityId)
        {
            return _context.Stream.Where(s => s.ActivityId == activityId).OrderBy(s=>s.Time);
        }


        public IQueryable<Activity> GetActivities(string userId)
        {
            return _context.Activity
                  .Where(a => a.Athlete.UserId == userId)
                  .Include(a => a.ActivityType);

        }

        public IEnumerable<ActivityDto> GetActivityDto(string userId)
        {
            return GetRecentActivity(userId, null);
        }


        public IEnumerable<ActivityDto> GetRecentActivity(string userId, int? returnedRows)
        {
            if (returnedRows == null)
                returnedRows = int.MaxValue;

            var activities = _context.Activity
                 .Where(a => a.Athlete.UserId == userId)
                 .Include(a => a.ActivityType)
                 .OrderByDescending(a => a.StartDateLocal)
                 .Take(returnedRows.Value)
                 .ToList();           

            List<ActivityDto> results = new List<ActivityDto>();

            foreach (Activity a in activities)
                results.Add(ActivityDto.CreateFromActivity(a));

            return results;
        }

        internal IEnumerable<ActivityDto> GetRecentActivity(List<ActivityDto> summaryActivities, int returnedRows)
        {
            return summaryActivities
                 .OrderByDescending(a => a.StartDateLocal)
                 .Take(returnedRows)
                 .ToList();
        }

        public bool DeleteActivityDetails(long activityId)
        {
            try
            {
                _context.Database.ExecuteSqlCommand("dbo.ActivityDeleteDetails @activityId", new SqlParameter("activityid", activityId));
            }
            catch (SqlException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return false;
            }

            return true;
        }

        public bool DeleteActivityStream(long activityId)
        {
            try
            {
                _context.Database.ExecuteSqlCommand("dbo.ActivityStreamDelete @activityId", new SqlParameter("activityid", activityId));
            }
            catch (SqlException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return false;
            }

            return true;
        }


        internal void UpdateActivity(Activity amended)
        {
            _context.Activity.Attach(amended);
            _context.Entry(amended).State = EntityState.Modified;
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

        public void AddStreamBulk(IEnumerable<Stream> s)
        {
            _context.BulkInsert(s);
        }

        #endregion

        public void AddOrUpdateGear(Gear g)
        {
            _context.Gear.AddOrUpdate(g);
        }

        public IEnumerable<CoordsDto> GetActivityCoords(long activityId)
        {
            return _context.Stream
                .Include(a => a.Activity)
                 .Where(s => s.ActivityId == activityId && s.Time % s.Activity.StreamStep == 0)
                 .OrderBy(s => s.Time)
                 .Select(s => new CoordsDto
                 {
                     lat = s.Latitude.Value,
                     lng = s.Longitude.Value
                 })

                 .ToList();
        }


        public GraphStreamDto GetActivityStreams(long activityId)
        {
            // by default unless told otherwise by parameter we'll use the steps set-up with the activity (so pass false).
            return GetActivityStreams(activityId, false);
        }


        public GraphStreamDto GetActivityStreams(long activityId, bool ignoreStep)
        {



            var activityStream = _context.Stream
                 .Include(a => a.Activity)
                 .Where(s => s.ActivityId == activityId && (ignoreStep ? true : s.Time % s.Activity.StreamStep == 0))
                 .Select(s => new
                 {
                     Time = s.Time,
                     Altitude = s.Altitude,
                     HeartRate = s.HeartRate,
                     Cadence = s.Cadence,
                     Watts = s.Watts
                 })
                  .OrderBy(s => s.Time)
                 .ToList();

            GraphStreamDto result = new GraphStreamDto();

            foreach (var s in activityStream)
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

        public IEnumerable<RunningTimesDto> GetBestTimes(string userId)
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
                          select new RunningTimesDto
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

        //public List<Stream> GetActivityStream(long activityId)
        //{
        //    return _context.Stream
        //               .Where(s => s.ActivityId == activityId)
        //               .OrderBy(s=>s.Time)
        //               .ToList();           
        //}



     
        //public ActivityPeakDetail GetActivityPeakDetail(int id)
        //{
        //    return _context.ActivityPeakDetail.Where(p => p.Id == id).FirstOrDefault();
        //}



        public IEnumerable<PeriodDto> ActivityByWeek(string userId, string activityType, DateTime start, DateTime end)
        {
            // get list of weeks in the period (to ensure we get full weeks date where the start and/or end date may be in the 
            // middle of a week
            var weeks = _context.Calendar
                .OrderBy(c => c.YearWeek)
                .Where(c => c.Date >= start && c.Date <= end)
                .Select(c => c.YearWeek)
                .Distinct()
                .ToList();

            // get activities which fall into the selected weeks.
            var activities = ActivitiesBySport(userId, activityType)
                .Where(r => r.Athlete.UserId == userId &&
                      weeks.Contains(r.Calendar.YearWeek))
                .Select(r => new
                {
                    Id = r.Id,
                    ActivityType = r.ActivityType.Description,
                    Period = r.Calendar.YearWeek,
                    Distance = r.Distance,
                    Label = r.Calendar.WeekLabel,
                })
            .ToList();

            // group the activities by week.
            var weeklyTotals = activities
                .GroupBy(r => new { Period = r.Period, Label = r.Label })
                .Select(a => new PeriodDto
                {
                    Period = a.Key.Period,
                    TotalDistance = Math.Round(a.Sum(d => d.Distance).ToMiles(), 1),
                    Number = a.Select(i => i.Id).Distinct().Count(),
                    Label = a.Key.Label
                })
                .ToList();

            // get list of weeks in the period which we'l use to check for weeks with zero activities which won't be
            // included in weeklyTotals.
            var dummyWeeks = _context.Calendar
                .OrderBy(c => c.YearWeek)
                .Where(c => c.Date >= start && c.Date <= end)
                .Select(c => new PeriodDto
                {
                    Period = c.YearWeek,
                    TotalDistance = 0,
                    Number = 0,
                    Label = c.WeekLabel
                }
                )
                .Distinct()
                .ToList();

            // merge to ensure we include weeks with no activities.
            var result = weeklyTotals
                .Union(dummyWeeks
                .Where(e => !weeklyTotals.Select(x => x.Period).Contains(e.Period)))
                .OrderBy(x => x.Period);

            return result;
        }

        internal void AddLap(Models.Lap lap)
        {
            _context.Lap.Add(lap);
        }


        public IEnumerable<LapDto> GetLaps(long activityId)
        {
            // split into two queries - first is Linq to sql, second is linq to objects  - need to split as can't format the TimeSpan on Linq to Sql
            var results = _context.Lap
                .Include(a => a.Activity)
                .Where(l => l.ActivityId == activityId).OrderBy(l => l.LapIndex)
                  .Select(l => new
                  {
                      Id = l.Id,
                      Type = PeakStreamType.Lap,
                      Selected = false,
                      Name = l.Name,
                      ElapsedTime = l.ElapsedTime,
                      StartIndex = l.StartIndex,
                      EndIndex = l.EndIndex,
                      SteppedStartIndex = l.StartIndex / l.Activity.StreamStep,
                      SteppedEndEnd = l.EndIndex / l.Activity.StreamStep,
                      StreamStep = l.Activity.StreamStep,
                      MovingTime = l.MovingTime,
                      LapIndex = l.LapIndex
                  })
                  .OrderBy(l => l.LapIndex)
                  .ToList()
                  .Select(l => new LapDto
                  {
                      Id = l.Id,
                      Type = PeakStreamType.Lap,
                      Selected = false,
                      Name = l.Name,
                      Value = l.ElapsedTime.ToString(@"hh\:mm\:ss"),
                      StartIndex = l.StartIndex,
                      EndIndex = l.EndIndex,
                      StreamStep = l.StreamStep,
                      SteppedStartIndex = l.SteppedStartIndex,
                      SteppedEndIndex = l.SteppedEndEnd,
                      MovingTime = l.MovingTime,
                      LapIndex=l.LapIndex
                  }).ToList();


            // if no laps then return to caller without trying to fix data.
            if (results.Count() == 0)
                return results;


            /*
            Code below is a fix for issue with Strava lap details.   If the activity starts in the users
            privacy zone then the StartIndex/EndIndex for the first lap will exclude any points in the privacy zone.
            This results in lap 1 being short by the time in the private zone and all other laps starting/finishing
            early by the same time. 

            Fix is to work out how many points should be in the first lap from the moving time and deduct the EndIndex
            value.  This will give the number of points missing.

            We can then adjust the end index of lap one and the start/end indexes of all other laps.
            */

            // get details of first lap
            LapDto lap1 = results[0];

            // workout how my points we're short by getting number of points we're expecting from the moving time
            // and deducting the number of points we've actually got.
            int shorageInStream = (int)lap1.MovingTime.TotalSeconds - lap1.EndIndex.Value;

            foreach (LapDto lap in results)
            {
                // for the first lap we don't need to adjust (StartIndex will be 0 and should remain 0).  For
                // other laps adjust.
                if (lap.LapIndex != 1)
                    lap.StartIndex += shorageInStream;

                // always adjust the end index.
                lap.EndIndex += shorageInStream;             
            }

            return results;
        }


        //public Lap GetLap(int id)
        //{
        //    return _context.Lap.Where(l => l.Id == id).FirstOrDefault();
        //}



        public IEnumerable<TimeDistanceBySportDto> GetTimeDistanceBySport(string userId, DateTime start, DateTime end)
        {
            // get activities which fall into the selected weeks.
            var activities = _context.Activity
                .Include(r => r.Calendar)
                .Include(r => r.ActivityType)
                .Include(r => r.Athlete)
                .Where(r => r.Athlete.UserId == userId &&
                r.Start >= start &&
                r.Start <= end &&
                r.MovingTime != null)
                .Select(r => new
                {
                    Duration = r.MovingTime.Value,
                    Distance = r.Distance,
                    Sport = r.ActivityType.IsRun ? "Run" : r.ActivityType.IsRide ? "Ride" : r.ActivityType.IsSwim ? "Swim" : "Other"
                })

            .ToList();

            // group the activities by week.
            var totalsBySport = activities
             .GroupBy(r => new { Sport = r.Sport })
                .Select(r => new TimeDistanceBySportDto
                {
                    Sport = r.Key.Sport,
                    Distance = r.Sum(d => d.Distance),
                    Duration = r.Sum(e => Convert.ToDecimal(e.Duration.TotalMinutes))
                })

                .ToList();


            foreach (TimeDistanceBySportDto t in totalsBySport)
            {
                TimeSpan duration = TimeSpan.FromMinutes((double)t.Duration);

                t.Sport = string.Format("{0} {1}:{2}:{3}", t.Sport,
                                                         ((duration.Days * 24) + duration.Hours).ToString(),
                                                         duration.Minutes.ToString().PadLeft(2, '0'),
                                                         duration.Seconds.ToString().PadLeft(2, '0'));
            }

            return totalsBySport;
        }

        public IEnumerable<LapDto> GetLapStream(long activityId, PeakStreamType streamType)
        {
            string units = DisplayLabel.PeakStreamTypeUnits(streamType);

            var result = _context.ActivityPeakDetail
              .Where(p => p.ActivityId == activityId && p.StreamType == streamType && 
                            _context.PeakStreamTypeDuration
                                .Where(d=>d.PeakStreamType == p.StreamType)
                                .Select(d=>d.Duration)
                                .ToList()
                                .Contains(p.Duration)
                    )
              .OrderBy(p => p.Duration)
              .Select(p => new LapDto
              {
                  Id = p.Id,
                  Type = streamType,
                  Selected = false,
                  Name = p.Duration.ToString(),
                  Value = p.Value.ToString(),
                  StartIndex = p.StartIndex.Value,       
                  EndIndex = p.EndIndex.Value,              
                  StreamStep = p.Activity.StreamStep,
                  SteppedStartIndex = p.StartIndex.Value / p.Activity.StreamStep,
                  SteppedEndIndex = p.EndIndex.Value / p.Activity.StreamStep,
              }).ToList();
            
            foreach (LapDto l in result)
            {
                l.Name = DisplayLabel.StreamDurationForDisplay(Convert.ToInt32(l.Name));
            }
            return result;
        }

        /// <summary>
        /// Query to generate a list of activities which match given criteria
        /// </summary>
        /// <param name="userId">ASP.NET Identity Id</param>
        /// <param name="sport">Run, Ride, Swim, Other, All</param>
        /// <param name="start">Beginning of date range</param>
        /// <param name="end">End of date range</param>
        /// <returns></returns>
        public IQueryable<ActivityDto> GetSportSummaryQuery(string userId, string sport, DateTime start, DateTime end)
        {
            IQueryable<ActivityDto> activityQuery = this.ActivitiesBySport(userId, sport)
                .Include(r => r.ActivityType)
                .Include(r => r.Athlete)
                .Where(r => r.Start >= start && r.Start <= end)
                    .Select(r => new ActivityDto
                    {
                        Id = r.Id,
                        Name = r.Name,
                        ActivityTypeId = r.ActivityTypeId,
                        MovingTime = r.MovingTime != null ? r.MovingTime.Value : new TimeSpan(0, 0, 0),
                        Distance = r.Distance,
                        SufferScore = r.SufferScore != null ? r.SufferScore.Value : 0,
                        Calories = r.Calories,
                        ElevationGain = r.ElevationGain,
                        Start = r.Start,
                        StartDateLocal = r.StartDateLocal,
                        HasMap = r.StartLatitude != null ? true : false,
                        IsRide = r.ActivityType.IsRide,
                        IsRun = r.ActivityType.IsRun,
                        IsSwim = r.ActivityType.IsSwim,
                        IsOther = r.ActivityType.IsOther,
                        DetailsDownloaded = r.DetailsDownloaded,
                        HasPowerMeter = r.HasPowerMeter
                    });

            return activityQuery;
        }


        //public SportSummaryDto GetSportSummary(string userId, string sport, DateTime start, DateTime end)
        //{
        //    return GetSportSummary(userId, sport, start, end, null);
        //}

        public SportSummaryDto GetSportSummary(string userId, string sport, DateTime start, DateTime end, List<ActivityDto> fullActivityList)
        {
            IEnumerable<ActivityDto> activities;

            if (fullActivityList == null)
                activities = GetSportSummaryQuery(userId, sport, start, end).ToList();
            else if (sport == "Ride")
                activities = fullActivityList.Where(r => r.IsRide && r.Start >= start && r.Start <= end).ToList();
            else if (sport == "Run")
                activities = fullActivityList.Where(r => r.IsRun && r.Start >= start && r.Start <= end).ToList();
            else if (sport == "Swim")
                activities = fullActivityList.Where(r => r.IsSwim && r.Start >= start && r.Start <= end).ToList();
            else if (sport == "Other")
                activities = fullActivityList.Where(r => r.IsOther && r.Start >= start && r.Start <= end).ToList();
            else
                activities = fullActivityList.Where(r => r.Start >= start && r.Start <= end).ToList();

            SportSummaryDto sportSummary = new SportSummaryDto();
            sportSummary.Sport = sport;
            sportSummary.Duration = TimeSpan.FromSeconds(activities.Sum(r => r.MovingTime.TotalSeconds));
            sportSummary.Distance = activities.Sum(r => r.Distance);
            sportSummary.SufferScore = activities.Sum(r => r.SufferScore);
            sportSummary.Calories = activities.Sum(r => r.Calories);
            sportSummary.ElevationGain = activities.Sum(r => r.ElevationGain).ToFeet();
            sportSummary.ActivityCount = activities.Count();

            return sportSummary;
        }

        public IQueryable<Activity> ActivitiesBySport(string userId, string sport)
        {
            // get activities which fall into the selected weeks.
            var activitiesQuery = _context.Activity
                .Include(r => r.ActivityType)
                .Include(r => r.Athlete)
                .Where(r => r.Athlete.UserId == userId);

            if (sport == "Ride")
                return activitiesQuery.Where(r => r.ActivityType.IsRide);
            else if (sport == "Run")
                return activitiesQuery.Where(r => r.ActivityType.IsRun);
            else if (sport == "Swim")
                return activitiesQuery.Where(r => r.ActivityType.IsSwim);
            else if (sport == "Other")
                return activitiesQuery.Where(r => r.ActivityType.IsOther);
            else
                return activitiesQuery;
        }
    }
}