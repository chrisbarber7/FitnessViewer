using AutoMapper;
using FitnessViewer.Infrastructure.Data;
using FitnessViewer.Infrastructure.enums;
using FitnessViewer.Infrastructure.Interfaces;
using FitnessViewer.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessViewer.Infrastructure.Models.Dto
{
    public class AthleteDashboardDto : AthleteDto
    {
        public static new AthleteDashboardDto Create(IUnitOfWork uow, string userId)
        {
            ApplicationDb context = new ApplicationDb();
                 

            AthleteDashboardDto dashboard = Mapper.Map<AthleteDashboardDto>(AthleteDto.Create(uow, userId));

            if (dashboard == null)
                return null;

            PeaksDtoRepository peaksRepo = new PeaksDtoRepository(context);
            RunningTimesDtoRepository timesRepo = new RunningTimesDtoRepository(context);
            ActivityDtoRepository activityRepo = new ActivityDtoRepository(context);
            SportSummaryDtoRepository sportRepo = new SportSummaryDtoRepository(context);
            WeightByDayDtoRepository weightRepo = new WeightByDayDtoRepository(context);

            // get a list of activities for the past 90 days which will be used to extract various summary information details.
            var summaryActivities = activityRepo.GetSportSummaryQuery(userId, "All", DateTime.Now.AddDays(-90), DateTime.Now).ToList();

            dashboard.PowerPeaks = peaksRepo.GetPeaks(userId, PeakStreamType.Power);
            dashboard.RunningTime = timesRepo.GetBestTimes(userId);
            dashboard.CurrentWeight = weightRepo.GetMetricDetails(userId, MetricType.Weight, 1)[0];
            dashboard.RecentActivity = activityRepo.GetRecentActivity(summaryActivities, 7);
            dashboard.Run7Day = sportRepo.GetSportSummary(userId, "Run", DateTime.Now.AddDays(-7), DateTime.Now, summaryActivities);
            dashboard.Bike7Day = sportRepo.GetSportSummary(userId, "Ride", DateTime.Now.AddDays(-7), DateTime.Now, summaryActivities);
            dashboard.Swim7Day = sportRepo.GetSportSummary(userId, "Swim", DateTime.Now.AddDays(-7), DateTime.Now, summaryActivities);
            dashboard.Other7Day = sportRepo.GetSportSummary(userId, "Other", DateTime.Now.AddDays(-7), DateTime.Now, summaryActivities);
            dashboard.All7Day = sportRepo.GetSportSummary(userId, "All", DateTime.Now.AddDays(-7), DateTime.Now, summaryActivities);
            dashboard.Run30Day = sportRepo.GetSportSummary(userId, "Run", DateTime.Now.AddDays(-30), DateTime.Now, summaryActivities);
            dashboard.Bike30Day = sportRepo.GetSportSummary(userId, "Ride", DateTime.Now.AddDays(-30), DateTime.Now, summaryActivities);
            dashboard.Swim30Day = sportRepo.GetSportSummary(userId, "Swim", DateTime.Now.AddDays(-30), DateTime.Now, summaryActivities);
            dashboard.Other30Day = sportRepo.GetSportSummary(userId, "Other", DateTime.Now.AddDays(-30), DateTime.Now, summaryActivities);
            dashboard.All30Day = sportRepo.GetSportSummary(userId, "All", DateTime.Now.AddDays(-30), DateTime.Now, summaryActivities);
            dashboard.Run90Day = sportRepo.GetSportSummary(userId, "Run", DateTime.Now.AddDays(-90), DateTime.Now, summaryActivities);
            dashboard.Bike90Day = sportRepo.GetSportSummary(userId, "Ride", DateTime.Now.AddDays(-90), DateTime.Now, summaryActivities);
            dashboard.Swim90Day = sportRepo.GetSportSummary(userId, "Swim", DateTime.Now.AddDays(-90), DateTime.Now, summaryActivities);
            dashboard.Other90Day = sportRepo.GetSportSummary(userId, "Other", DateTime.Now.AddDays(-90), DateTime.Now, summaryActivities);
            dashboard.All90Day = sportRepo.GetSportSummary(userId, "All", DateTime.Now.AddDays(-90), DateTime.Now, summaryActivities);


            return dashboard;
        }

        public IEnumerable<PeaksDto> PowerPeaks { get; set; }
        public IEnumerable<RunningTimesDto> RunningTime { get; internal set; }
        public WeightByDayDto CurrentWeight { get; set; }
        public IEnumerable<ActivityDto> RecentActivity { get; set; }

        public SportSummaryDto Run7Day { get; set; }
        public SportSummaryDto Run30Day { get; set; }
        public SportSummaryDto Run90Day { get; set; }

        public SportSummaryDto Bike7Day { get; set; }
        public SportSummaryDto Bike30Day { get; set; }
        public SportSummaryDto Bike90Day { get; set; }

        public SportSummaryDto Swim7Day { get; set; }
        public SportSummaryDto Swim30Day { get; set; }
        public SportSummaryDto Swim90Day { get; set; }

        public SportSummaryDto Other7Day { get; set; }
        public SportSummaryDto Other30Day { get; set; }
        public SportSummaryDto Other90Day { get; set; }

        public SportSummaryDto All7Day { get; set; }
        public SportSummaryDto All30Day { get; set; }
        public SportSummaryDto All90Day { get; set; }
    }
}




