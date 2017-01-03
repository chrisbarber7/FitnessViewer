using FitnessViewer.Infrastructure.Data;
using FitnessViewer.Infrastructure.enums;
using FitnessViewer.Infrastructure.Helpers;
using FitnessViewer.Infrastructure.Interfaces;
using FitnessViewer.Infrastructure.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Linq.Expressions;
using FitnessViewer.Infrastructure.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessViewer.Infrastructure.Repository
{
    public class PeriodDtoRepository : DtoRepository, IPeriodDtoRepository
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

        public IEnumerable<PeriodDto> ActivityByWeek(string userId, SportType sport, DateTime start, DateTime end)
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
            var activities = _activityrepo.ActivitiesBySport(userId, sport)
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
                    MaximumDistance = Math.Round(a.Max(d => d.Distance).ToMiles(), 1),
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
                    MaximumDistance = 0,
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



        public IEnumerable<ActivityPeaksPeriodDto> PeaksByMonth(string userId, DateTime start, DateTime end)
        {
            // get list of weeks in the period (to ensure we get full weeks date where the start and/or end date may be in the 
            // middle of a week
            var months = _context.Calendar
                .OrderBy(c => c.YearMonth)
                .Where(c => c.Date >= start && c.Date <= end)
                .Select(c => c.YearMonth)
                .Distinct()
                .ToList();

            var activityPeaks = _context.ActivityPeak
                .Where(r => r.Activity.Athlete.UserId == userId && r.Activity.ActivityType.IsRide && months.Contains(r.Activity.Calendar.YearMonth))
                .Select(r => new
                {
                    Id = r.ActivityId,
                    Period = r.Activity.Calendar.YearMonth,
                    Label = r.Activity.Calendar.MonthName.Substring(0, 3) + " " + r.Activity.Calendar.Year.ToString(),
                    Peak5 = r.Peak5,
                    Peak30 = r.Peak30,
                    Peak60 = r.Peak60,
                    Peak300 = r.Peak300,
                    Peak1200 = r.Peak1200,
                    Peak3600 = r.Peak3600
                })
            .ToList();

            // group the activities by week.
            var monthlyPeaks = activityPeaks
                .GroupBy(r => new { Period = r.Period, Label = r.Label })
                .Select(a => new ActivityPeaksPeriodDto
                {
                    Period = a.Key.Period,
                    Peak5 = a.Max(d => d.Peak5),
                    Peak30 = a.Max(d => d.Peak30),
                    Peak60 = a.Max(d => d.Peak60),
                    Peak300 = a.Max(d => d.Peak300),
                    Peak1200 = a.Max(d => d.Peak1200),
                    Peak3600 = a.Max(d => d.Peak3600),
                    Label = a.Key.Label
                })
                .ToList();

            // get list of months in the period which we'l use to check for weeks with zero activities which won't be
            // included in monthlyTotals.
            var dummyMonths = _context.Calendar
                .OrderBy(c => c.YearMonth)
                .Where(c => c.Date >= start && c.Date <= end)
                .Select(c => new ActivityPeaksPeriodDto
                {
                    Period = c.YearMonth,
                    Peak5 = 0,
                    Peak30 = 0,
                    Peak60 = 0,
                    Peak300 = 0,
                    Peak1200 = 0,
                    Peak3600 = 0,
                    Label = c.MonthName.Substring(0, 3) + " " + c.Year.ToString(),
                }
                )
                .Distinct()
                .ToList();

            // merge to ensure we include weeks with no activities.
            var result = monthlyPeaks
                .Union(dummyMonths
                .Where(e => !monthlyPeaks.Select(x => x.Period).Contains(e.Period)))
                .OrderBy(x => x.Period);

            return result;
        }

        public IEnumerable<PowerCurveDto> PowerCurve(string userId, DateTime start, DateTime end)
        {
            var powerCurve = _context.ActivityPeakDetail
                .Where(r => r.Activity.Athlete.UserId == userId &&
                            r.Activity.ActivityType.IsRide &&
                            r.Value.HasValue &&
                            r.Activity.Start >= start &&
                            r.Activity.Start <= end)
                .GroupBy(r => new { Duration = r.Duration })
                .Select(p => new PowerCurveDto
                    {
                     Duration = p.Key.Duration,
                        Watts = p.Max(g => g.Value.Value)
                    })
                .OrderBy(r => r.Duration)
                .ToList();

            return powerCurve;
        }
    }
}