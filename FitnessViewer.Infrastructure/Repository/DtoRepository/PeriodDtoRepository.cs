using FitnessViewer.Infrastructure.Data;
using FitnessViewer.Infrastructure.Helpers;
using FitnessViewer.Infrastructure.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FitnessViewer.Infrastructure.Repository
{
    public class PeriodDtoRepository : DtoRepository
    {
        private ActivityRepository _activityrepo;

        public PeriodDtoRepository() : base()
        {
            _activityrepo = new ActivityRepository(_context);
        }
        public PeriodDtoRepository(ApplicationDb context) : base(context)
        {
            _activityrepo = new ActivityRepository(context);
        }

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
            var activities = _activityrepo.ActivitiesBySport(userId, activityType)
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
    }
}
