using FitnessViewer.Infrastructure.Data;
using FitnessViewer.Infrastructure.enums;
using FitnessViewer.Infrastructure.Helpers;
using FitnessViewer.Infrastructure.Intefaces;
using FitnessViewer.Infrastructure.Models;
using FitnessViewer.Infrastructure.Models.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace FitnessViewer.Infrastructure.Repository
{
    public class ActivityDtoRepository : DtoRepository, IActivityDtoRepository
    {
        public ActivityDtoRepository(ApplicationDb context) : base(context)
        {
        }

        public ActivityDtoRepository() : base() { }

        public IEnumerable<ActivityDto> GetActivityDto(string userId)
        {
            return GetRecentActivity(userId, null);
        }


        /// <summary>
        /// Query to generate a list of activities which match given criteria
        /// </summary>
        /// <param name="userId">ASP.NET Identity Id</param>
        /// <param name="sport">Run, Ride, Swim, Other, All</param>
        /// <param name="start">Beginning of date range</param>
        /// <param name="end">End of date range</param>
        /// <returns></returns>
        public IQueryable<ActivityDto> GetSportSummaryQuery(string userId, SportType sport, DateTime start, DateTime end)
        {
            ActivityRepository activityRepo = new ActivityRepository(_context);
            
            IQueryable<ActivityDto> activityQuery = activityRepo.ActivitiesBySport(userId, sport)
                .Include(r => r.ActivityType)
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
                        HasPowerMeter = r.HasPowerMeter,
                        TSS = r.TSS != null ? r.TSS : 0
                    });

            return activityQuery;
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
        
        public List<KeyValuePair<DateTime, decimal>> GetDailyTSS(string userId, SportType sport, DateTime start, DateTime end)
        {
            // two select statement as can't call the KeyValuePair constructor from Linq To SQL.
            ActivityRepository activityRepo = new ActivityRepository(_context);
            var results = activityRepo.ActivitiesBySport(userId, sport).Where(a => a.StartDate >= start && a.StartDate <= end&& a.TSS.HasValue)
                   .GroupBy(a => a.Start)
                   .Select(a => new { Key = a.Key, Value = a.Sum(g => g.TSS.Value) })
                   .ToList();
             
                   return results
                   .Select(a => new KeyValuePair<DateTime, decimal>(a.Key, a.Value))
                   .ToList();
         }

        public List<YearlyDetailsDayInfo> GetYearToDateInfo(string userId, int? year=null)
        {
            return _context.Activity
                      .Where(c => c.ActivityType.IsRide && c.Athlete.UserId == userId && year == null ? true : c.StartDate.Year == year.Value)
                      .GroupBy(c => new { Date = c.Start })
                      .Select(a => new YearlyDetailsDayInfo { Date = a.Key.Date, Sport = SportType.Ride, Distance = a.Sum(g => g.Distance) })
                   .Union(

                  _context.Activity
                      .Where(c => c.ActivityType.IsRun && c.Athlete.UserId == userId && year == null ? true : c.StartDate.Year == year.Value)
                      .GroupBy(c => new { Date = c.Start })
                      .Select(a => new YearlyDetailsDayInfo { Date = a.Key.Date, Sport = SportType.Run, Distance = a.Sum(g => g.Distance) })
                  )
                  .Union(
                    _context.Activity
                      .Where(c => c.ActivityType.IsSwim && c.Athlete.UserId == userId && year == null ? true : c.StartDate.Year == year.Value)
                      .GroupBy(c => new { Date = c.Start })
                      .Select(a => new YearlyDetailsDayInfo { Date = a.Key.Date, Sport = SportType.Swim, Distance = a.Sum(g => g.Distance) })
                  )
                  .Union
              (
                    _context.Activity
                      .Where(c => c.ActivityType.IsOther && c.Athlete.UserId == userId && year == null ? true : c.StartDate.Year == year.Value)
                      .GroupBy(c => new { Date = c.Start })
                      .Select(a => new YearlyDetailsDayInfo { Date = a.Key.Date, Sport = SportType.Other, Distance = a.Sum(g => g.Distance) })
                  )
                  .ToList();
        }
    }


}
