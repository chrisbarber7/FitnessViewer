using FitnessViewer.Infrastructure.Data;
using FitnessViewer.Infrastructure.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace FitnessViewer.Infrastructure.Repository
{
    public class TimeDistanceBySportRepository : DtoRepository
    {
        public TimeDistanceBySportRepository() : base(){ }
        public TimeDistanceBySportRepository(ApplicationDb context) : base(context)
        {
        }
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

            // need to make sure that each sport has at least a single row so
            // that it'll be included in the results.
            if (totalsBySport.Select(r => r.Sport == "Run").Count() == 0)
                totalsBySport.Add(new TimeDistanceBySportDto() { Sport = "Run", Distance = 0, Duration = 0 });

            if (totalsBySport.Select(r => r.Sport == "Ride").Count() == 0)
                totalsBySport.Add(new TimeDistanceBySportDto() { Sport = "Ride", Distance = 0, Duration = 0 });


            if (totalsBySport.Select(r => r.Sport == "Swim").Count() == 0)
                totalsBySport.Add(new TimeDistanceBySportDto() { Sport = "Swim", Distance = 0, Duration = 0 });

            if (totalsBySport.Select(r => r.Sport == "Other").Count() == 0)
                totalsBySport.Add(new TimeDistanceBySportDto() { Sport = "Other", Distance = 0, Duration = 0 });

            foreach (TimeDistanceBySportDto t in totalsBySport)
            {
                TimeSpan duration = TimeSpan.FromMinutes((double)t.Duration);

                t.Sport = string.Format("{0} {1}:{2}:{3}", t.Sport,
                                                         ((duration.Days * 24) + duration.Hours).ToString(),
                                                         duration.Minutes.ToString().PadLeft(2, '0'),
                                                         duration.Seconds.ToString().PadLeft(2, '0'));
            }

            return totalsBySport.OrderBy(s => s.Sport);
        }

    }
}
