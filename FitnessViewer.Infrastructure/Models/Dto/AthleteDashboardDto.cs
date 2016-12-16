using FitnessViewer.Infrastructure.Data;
using FitnessViewer.Infrastructure.enums;
using FitnessViewer.Infrastructure.Helpers;
using FitnessViewer.Infrastructure.Interfaces;
using FitnessViewer.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FitnessViewer.Infrastructure.Models.Dto
{
    public class AthleteDashboardDto : AthleteDto
    {
        private IUnitOfWork _uow;
        private string _userId;
        private List<ActivityDto> _summaryActivities;
        private TrainingLoad _trainingLoad;
        private RunningTimesDtoRepository _timesRepo;

        public AthleteDashboardDto(IUnitOfWork uow, string userId)
        {
            _uow = uow;
            _userId = userId;
        }

        public bool Populate()
        {
            AthleteDto athlete = AthleteDto.Create(_uow, _userId);

            if (athlete == null)
                return false;

            this.FirstName = athlete.FirstName;
            this.LastName = athlete.LastName;

            ApplicationDb context = new ApplicationDb();

            PeaksDtoRepository peaksRepo = new PeaksDtoRepository(context);
            _timesRepo = new RunningTimesDtoRepository(context);
            ActivityDtoRepository activityRepo = new ActivityDtoRepository(context);
            WeightByDayDtoRepository weightRepo = new WeightByDayDtoRepository(context);

            // get a list of activities for the past 90 days which will be used to extract various summary information details.
            _summaryActivities = activityRepo.GetSportSummaryQuery(_userId, SportType.All, DateTime.Now.AddDays(-90), DateTime.Now).ToList();

            PowerPeaks = peaksRepo.GetPeaks(_userId, PeakStreamType.Power);
            RunningTime = _timesRepo.GetBestTimes(_userId);
            CurrentWeight = weightRepo.GetMetricDetails(_userId, MetricType.Weight, 1)[0];
            RecentActivity = activityRepo.GetRecentActivity(_summaryActivities, 7);
            
            _trainingLoad = new TrainingLoad(activityRepo);
            // need to go back the highest number of days we're interested in plus 42 days for LongTerm training load duration
            // and an extra day to get a seed value.   Add an extra day to the end to hold current form.
            _trainingLoad.Setup(_userId, DateTime.Now.AddDays(-365 - 42 - 1), DateTime.Now.AddDays(1));
            _trainingLoad.Calculate(SportType.Ride);

            DateTime start = DateTime.Now.AddDays(30 * -1).Date;
            DateTime end = DateTime.Now.Date;

            RunSummary = DashboardSportSummary.Create(_userId, SportType.Run, start, end, _summaryActivities);
            BikeSummary = DashboardSportSummary.Create(_userId, SportType.Ride, start, end, _summaryActivities);
            SwimSummary = DashboardSportSummary.Create(_userId, SportType.Swim, start, end, _summaryActivities);
            OtherSportSummary = DashboardSportSummary.Create(_userId, SportType.Other, start, end, _summaryActivities);
            AllSportSummary = DashboardSportSummary.Create(_userId, SportType.All, start, end, _summaryActivities);

            return true;
        }


        public IEnumerable<PeaksDto> PowerPeaks { get; set; }
        public IEnumerable<RunningTimesDto> RunningTime { get; internal set; }
        public WeightByDayDto CurrentWeight { get; set; }
        public IEnumerable<ActivityDto> RecentActivity { get; set; }

        public SportSummaryDto RunSummary { get; set; }

        public SportSummaryDto BikeSummary { get; set; }

        public SportSummaryDto SwimSummary { get; set; }

        public SportSummaryDto OtherSportSummary { get; set; }

        public SportSummaryDto AllSportSummary { get; set; }
    

        public object DayValuesForChart()
        {

            List<string> date = new List<string>();
            List<string> longTermStress = new List<string>();
            List<string> shortTermStress = new List<string>();

            foreach (TrainingLoadDay d in _trainingLoad.DayValues.Where(d => d.Date >= DateTime.Now.AddDays(-90) && d.Date <= DateTime.Now.AddDays(1)).ToList())
            {
                date.Add(d.Date.ToShortDateString());
                longTermStress.Add(d.LongTermLoad.ToString());
                shortTermStress.Add(d.ShortTermLoad.ToString());
            }

            var chart = new
            {
                Date = date,
                LongTermLoad = longTermStress,
                ShortTermLoad = shortTermStress
            };

            return chart;
        }


  
    }
}




