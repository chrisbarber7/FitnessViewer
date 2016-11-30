using FitnessViewer.Infrastructure.Data;
using FitnessViewer.Infrastructure.Helpers;
using FitnessViewer.Infrastructure.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FitnessViewer.Infrastructure.Repository
{
    public class SportSummaryDtoRepository : DtoRepository
    {
        public SportSummaryDtoRepository(ApplicationDb context) : base(context)
        {
        }

        public SportSummaryDto GetSportSummary(string userId, string sport, DateTime start, DateTime end, List<ActivityDto> fullActivityList)
        {
            IEnumerable<ActivityDto> activities;

            SportSummaryDto sportSummary = new SportSummaryDto();

            if (fullActivityList == null)
            {
                ActivityDtoRepository activityDtoRepo = new ActivityDtoRepository(_context);
                activities = activityDtoRepo.GetSportSummaryQuery(userId, sport, start, end).ToList();
            }
            else if (sport == "Ride")
            {
                activities = fullActivityList.Where(r => r.IsRide && r.Start >= start && r.Start <= end).ToList();
                sportSummary.IsRide = true;
            }
            else if (sport == "Run")
            {
                activities = fullActivityList.Where(r => r.IsRun && r.Start >= start && r.Start <= end).ToList();
                sportSummary.IsRun = true;
            }
            else if (sport == "Swim")
            {
                activities = fullActivityList.Where(r => r.IsSwim && r.Start >= start && r.Start <= end).ToList();
                sportSummary.IsSwim = true;
            }
            else if (sport == "Other")
            {
                activities = fullActivityList.Where(r => r.IsOther && r.Start >= start && r.Start <= end).ToList();
                sportSummary.IsOther = true;
            }
            else
                activities = fullActivityList.Where(r => r.Start >= start && r.Start <= end).ToList();
            
            sportSummary.Sport = sport;
            sportSummary.Duration = TimeSpan.FromSeconds(activities.Sum(r => r.MovingTime.TotalSeconds));
            sportSummary.Distance = activities.Sum(r => r.Distance);
            sportSummary.SufferScore = activities.Sum(r => r.SufferScore);
            sportSummary.Calories = activities.Sum(r => r.Calories);
            sportSummary.ElevationGain = activities.Sum(r => r.ElevationGain).ToFeet();
            sportSummary.ActivityCount = activities.Count();
        
            return sportSummary;
        }

    }
}
