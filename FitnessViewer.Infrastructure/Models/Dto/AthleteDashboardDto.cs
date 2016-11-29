using AutoMapper;
using FitnessViewer.Infrastructure.Data;
using FitnessViewer.Infrastructure.enums;
using FitnessViewer.Infrastructure.Helpers;
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
        private IUnitOfWork _uow;
        private string _userId;


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
            RunningTimesDtoRepository timesRepo = new RunningTimesDtoRepository(context);
            ActivityDtoRepository activityRepo = new ActivityDtoRepository(context);
            SportSummaryDtoRepository sportRepo = new SportSummaryDtoRepository(context);
            WeightByDayDtoRepository weightRepo = new WeightByDayDtoRepository(context);

            // get a list of activities for the past 90 days which will be used to extract various summary information details.
            var summaryActivities = activityRepo.GetSportSummaryQuery(_userId, "All", DateTime.Now.AddDays(-90), DateTime.Now).ToList();

            PowerPeaks = peaksRepo.GetPeaks(_userId, PeakStreamType.Power);
            RunningTime = timesRepo.GetBestTimes(_userId);
            CurrentWeight = weightRepo.GetMetricDetails(_userId, MetricType.Weight, 1)[0];
            RecentActivity = activityRepo.GetRecentActivity(summaryActivities, 7);

            Run7Day = sportRepo.GetSportSummary(_userId, "Run", DateTime.Now.AddDays(-7), DateTime.Now, summaryActivities);
            Bike7Day = sportRepo.GetSportSummary(_userId, "Ride", DateTime.Now.AddDays(-7), DateTime.Now, summaryActivities);
            Swim7Day = sportRepo.GetSportSummary(_userId, "Swim", DateTime.Now.AddDays(-7), DateTime.Now, summaryActivities);
            Other7Day = sportRepo.GetSportSummary(_userId, "Other", DateTime.Now.AddDays(-7), DateTime.Now, summaryActivities);
            All7Day = sportRepo.GetSportSummary(_userId, "All", DateTime.Now.AddDays(-7), DateTime.Now, summaryActivities);

            Run30Day = sportRepo.GetSportSummary(_userId, "Run", DateTime.Now.AddDays(-30), DateTime.Now, summaryActivities);
            Bike30Day = sportRepo.GetSportSummary(_userId, "Ride", DateTime.Now.AddDays(-30), DateTime.Now, summaryActivities);
            Swim30Day = sportRepo.GetSportSummary(_userId, "Swim", DateTime.Now.AddDays(-30), DateTime.Now, summaryActivities);
            Other30Day = sportRepo.GetSportSummary(_userId, "Other", DateTime.Now.AddDays(-30), DateTime.Now, summaryActivities);
            All30Day = sportRepo.GetSportSummary(_userId, "All", DateTime.Now.AddDays(-30), DateTime.Now, summaryActivities);

            Run90Day = sportRepo.GetSportSummary(_userId, "Run", DateTime.Now.AddDays(-90), DateTime.Now, summaryActivities);
            Bike90Day = sportRepo.GetSportSummary(_userId, "Ride", DateTime.Now.AddDays(-90), DateTime.Now, summaryActivities);
            Swim90Day = sportRepo.GetSportSummary(_userId, "Swim", DateTime.Now.AddDays(-90), DateTime.Now, summaryActivities);
            Other90Day = sportRepo.GetSportSummary(_userId, "Other", DateTime.Now.AddDays(-90), DateTime.Now, summaryActivities);
            All90Day = sportRepo.GetSportSummary(_userId, "All", DateTime.Now.AddDays(-90), DateTime.Now, summaryActivities);

   
            Bike7Day.Peak1 = ExtractPeak( 7, 5);
            Bike7Day.Peak2 = ExtractPeak( 7, 60);
            Bike7Day.Peak3 = ExtractPeak( 7, 1200);
            Bike7Day.Peak4 = ExtractPeak(7, 3600);

            Bike30Day.Peak1 = ExtractPeak(30, 5);
            Bike30Day.Peak2 = ExtractPeak(30, 60);
            Bike30Day.Peak3 = ExtractPeak(30, 1200);
            Bike30Day.Peak4 = ExtractPeak(30, 3600);

            Bike90Day.Peak1 = ExtractPeak(90, 5);
            Bike90Day.Peak2 = ExtractPeak(90, 60);
            Bike90Day.Peak3 = ExtractPeak(90, 1200);
            Bike90Day.Peak4 = ExtractPeak(90, 3600);

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
    }
}




