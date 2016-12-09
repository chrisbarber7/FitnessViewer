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
             _summaryActivities = activityRepo.GetSportSummaryQuery(_userId, "All", DateTime.Now.AddDays(-90), DateTime.Now).ToList();

            PowerPeaks = peaksRepo.GetPeaks(_userId, PeakStreamType.Power);
            RunningTime = _timesRepo.GetBestTimes(_userId);
            CurrentWeight = weightRepo.GetMetricDetails(_userId, MetricType.Weight, 1)[0];
            RecentActivity = activityRepo.GetRecentActivity(_summaryActivities, 7);


             _trainingLoad = new TrainingLoad(activityRepo);
            // need to go back the highest number of days we're interested in plus 42 days for LongTerm training load duration
            // and an extra day to get a seed value.   Add an extra day to the end to hold current form.
            _trainingLoad.Setup(_userId, DateTime.Now.AddDays(-365 - 42 - 1), DateTime.Now.AddDays(1));
            _trainingLoad.Calculate("Ride");

            Run7Day = GetSportSummary("Run", 7);
            Bike7Day = GetSportSummary("Ride", 7);
            Swim7Day = GetSportSummary("Swim", 7);
            Other7Day = GetSportSummary("Other", 7);
            All7Day = GetSportSummary("All", 7);

            Run30Day = GetSportSummary("Run", 30);
            Bike30Day = GetSportSummary("Ride", 30);
            Swim30Day = GetSportSummary("Swim", 30);
            Other30Day = GetSportSummary("Other", 30);
            All30Day = GetSportSummary("All", 30);

            Run90Day = GetSportSummary("Run", 90);
            Bike90Day = GetSportSummary("Ride", 90);
            Swim90Day = GetSportSummary("Swim", 90);
            Other90Day = GetSportSummary("Other", 90);
            All90Day = GetSportSummary("All", 90);

            return true;
        }

        private  KeyValuePair<string,string> ExtractPeak(int day, int duration)
        {
            foreach (var d in PowerPeaks)
            {
                if (d.Days == day)
                {
                    foreach (var p in d.DurationPeaks)
                        if (p.Duration == duration)
                            return new KeyValuePair<string, string>(DisplayLabel.ShortStreamDurationForDisplay(duration),
                                  string.Format("{0}{1}", p.Peak.ToString(), DisplayLabel.PeakStreamTypeUnits(PeakStreamType.Power)));
                }
            }
             return new KeyValuePair<string, string>(DisplayLabel.ShortStreamDurationForDisplay(duration), "-");
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


        private SportSummaryDto GetSportSummary(string sport, int days)
        {
            DateTime start = DateTime.Now.AddDays(days*-1).Date;
            DateTime end = DateTime.Now.Date;

            IEnumerable<ActivityDto> activities;

            SportSummaryDto sportSummary = new SportSummaryDto();

            if (sport == "Ride")
            {
                activities = _summaryActivities.Where(r => r.IsRide && r.Start >= start && r.Start <= end).ToList();
                sportSummary.IsRide = true;

                sportSummary.Peak1 = ExtractPeak(days, 5);
                sportSummary.Peak2 = ExtractPeak(days, 60);
                sportSummary.Peak3 = ExtractPeak(days, 1200);
                sportSummary.Peak4 = ExtractPeak(days, 3600);
            }
            else if (sport == "Run")
            {
                activities = _summaryActivities.Where(r => r.IsRun && r.Start >= start && r.Start <= end).ToList();
                sportSummary.IsRun = true;

                var bestEfforts = _timesRepo.GetBestTimes(_userId, start, end);

                var OneKm = bestEfforts.Where(b => b.Distance == 1000).FirstOrDefault();
                if (OneKm != null)
                sportSummary.Peak1 = new KeyValuePair<string, string>(OneKm.DistanceName, OneKm.AveragePace.ToMinSec());

                var OneMile = bestEfforts.Where(b => b.Distance == 1609).FirstOrDefault();
                if (OneMile != null)
                    sportSummary.Peak2 = new KeyValuePair<string, string>(OneMile.DistanceName, OneMile.AveragePace.ToMinSec());

                var FiveKm = bestEfforts.Where(b => b.Distance == 5000).FirstOrDefault();
                if (FiveKm != null)
                    sportSummary.Peak3 = new KeyValuePair<string, string>(FiveKm.DistanceName, FiveKm.AveragePace.ToMinSec());

                var TenKm = bestEfforts.Where(b => b.Distance == 10000).FirstOrDefault();
                if (TenKm != null)
                    sportSummary.Peak4 = new KeyValuePair<string, string>(TenKm.DistanceName, TenKm.AveragePace.ToMinSec());

            }
            else if (sport == "Swim")
            {
                activities = _summaryActivities.Where(r => r.IsSwim && r.Start >= start && r.Start <= end).ToList();
                sportSummary.IsSwim = true;
            }
            else if (sport == "Other")
            {
                activities = _summaryActivities.Where(r => r.IsOther && r.Start >= start && r.Start <= end).ToList();
                sportSummary.IsOther = true;
            }
            else
                activities = _summaryActivities.Where(r => r.Start >= start && r.Start <= end).ToList();

            sportSummary.Sport = sport;
            sportSummary.Duration = TimeSpan.FromSeconds(activities.Sum(r => r.MovingTime.TotalSeconds));
            sportSummary.Distance = activities.Sum(r => r.Distance);
            sportSummary.SufferScore = activities.Sum(r => r.SufferScore);
            sportSummary.Calories = activities.Sum(r => r.Calories);
            sportSummary.ElevationGain = activities.Sum(r => r.ElevationGain).ToFeet();
            sportSummary.ActivityCount = activities.Count();
            sportSummary.TSS = activities.Sum(r => r.TSS);
            sportSummary.TrainingLoadChartName = string.Format("{0}{1}Chart", sport, (end - start).TotalDays.ToString());

            return sportSummary;
        }
    }
}




