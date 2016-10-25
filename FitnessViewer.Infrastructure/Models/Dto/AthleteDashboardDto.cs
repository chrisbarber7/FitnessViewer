using AutoMapper;
using FitnessViewer.Infrastructure.Data;
using FitnessViewer.Infrastructure.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessViewer.Infrastructure.Models.Dto
{
    public class AthleteDashboardDto : AthleteDto
    {
        public static new AthleteDashboardDto Create(UnitOfWork uow, string userId)
        {
            AthleteDashboardDto dashboard = Mapper.Map<AthleteDashboardDto>(AthleteDto.Create(uow, userId));

            // get a list of activities for the past 90 days which will be used to extract various summary information details.
            var summaryActivities = uow.Activity.GetSportSummaryQuery(userId, "All", DateTime.Now.AddDays(-90), DateTime.Now).ToList();

            dashboard.PowerPeaks = uow.Analysis.GetPeaks(userId, PeakStreamType.Power);
            dashboard.RunningTime = uow.Activity.GetBestTimes(userId);
            dashboard.CurrentWeight = uow.Metrics.GetWeightDetails(userId, 1)[0];
            dashboard.RecentActivity = uow.Activity.GetRecentActivity(summaryActivities, 7);
            dashboard.Run7Day = uow.Activity.GetSportSummary(userId, "Run", DateTime.Now.AddDays(-7), DateTime.Now, summaryActivities);
            dashboard.Bike7Day = uow.Activity.GetSportSummary(userId, "Ride", DateTime.Now.AddDays(-7), DateTime.Now, summaryActivities);
            dashboard.Swim7Day = uow.Activity.GetSportSummary(userId, "Swim", DateTime.Now.AddDays(-7), DateTime.Now, summaryActivities);
            dashboard.Other7Day = uow.Activity.GetSportSummary(userId, "Other", DateTime.Now.AddDays(-7), DateTime.Now, summaryActivities);
            dashboard.All7Day = uow.Activity.GetSportSummary(userId, "All", DateTime.Now.AddDays(-7), DateTime.Now, summaryActivities);
            dashboard.Run30Day = uow.Activity.GetSportSummary(userId, "Run", DateTime.Now.AddDays(-30), DateTime.Now, summaryActivities);
            dashboard.Bike30Day = uow.Activity.GetSportSummary(userId, "Ride", DateTime.Now.AddDays(-30), DateTime.Now, summaryActivities);
            dashboard.Swim30Day = uow.Activity.GetSportSummary(userId, "Swim", DateTime.Now.AddDays(-30), DateTime.Now, summaryActivities);
            dashboard.Other30Day = uow.Activity.GetSportSummary(userId, "Other", DateTime.Now.AddDays(-30), DateTime.Now, summaryActivities);
            dashboard.All30Day = uow.Activity.GetSportSummary(userId, "All", DateTime.Now.AddDays(-30), DateTime.Now, summaryActivities);
            
            return dashboard;
        }

        public IEnumerable<PeaksDto> PowerPeaks { get; set; }
        public IEnumerable<RunningTimesDto> RunningTime { get; internal set; }
        public WeightByDayDto CurrentWeight { get; set; }
        public IEnumerable<ActivityDto> RecentActivity { get; set; }

        public SportSummaryDto Run7Day { get; set; }
        public SportSummaryDto Run30Day { get; set; }

        public SportSummaryDto Bike7Day { get; set; }
        public SportSummaryDto Bike30Day { get; set; }

        public SportSummaryDto Swim7Day { get; set; }
        public SportSummaryDto Swim30Day { get; set; }

        public SportSummaryDto Other7Day { get; set; }
        public SportSummaryDto Other30Day { get; set; }

        public SportSummaryDto All7Day { get; set; }
        public SportSummaryDto All30Day { get; set; }
    }
}




