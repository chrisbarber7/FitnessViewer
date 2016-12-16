using FitnessViewer.Infrastructure.Data;
using FitnessViewer.Infrastructure.enums;
using FitnessViewer.Infrastructure.Intefaces;
using FitnessViewer.Infrastructure.Interfaces;
using FitnessViewer.Infrastructure.Models.Dto;
using FitnessViewer.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FitnessViewer.Infrastructure.Helpers
{
    public class DashboardSportSummary
    {
        private IRunningTimesDtoRepository _timesRepo;
        private IActivityDtoRepository _activityRepo;
        private ApplicationDb _context;
        public string UserId { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public SportType Sport { get; set; }

        private List<ActivityDto> _summaryActivities;

        private DashboardSportSummary()
        {
            _context = new ApplicationDb();
            _timesRepo = new RunningTimesDtoRepository(_context);
            _activityRepo = new ActivityDtoRepository(_context);
        }

        public static SportSummaryDto Create(string userId, SportType sport, DateTime start, DateTime end)
        {
            return Create(userId, sport, start, end, null);
        }

        public static SportSummaryDto Create(string userId, SportType sport, DateTime start, DateTime end, List<ActivityDto> activities)
        {
            DashboardSportSummary summary = new DashboardSportSummary();
            summary.UserId = userId;
            summary.Sport = sport;
            summary.Start = start;
            summary.End = end;

            if (activities != null)
                summary._summaryActivities = activities;
            else
                summary._summaryActivities = summary._activityRepo.GetSportSummaryQuery(userId, sport, start, end).ToList();

            return summary.GetSportSummary();
        }


        private SportSummaryDto GetSportSummary()
        {


            IEnumerable<ActivityDto> sportActivitiesDuringPeriod;

            SportSummaryDto sportSummary = new SportSummaryDto();

            if (Sport == SportType.Ride)
            {
                sportActivitiesDuringPeriod = _summaryActivities.Where(r => r.IsRide && r.Start >= Start && r.Start <= End).ToList();
                sportSummary.IsRide = true;
                GetPowerPeakSummary(sportSummary);

            }
            else if (Sport == SportType.Run)
            {
                sportActivitiesDuringPeriod = _summaryActivities.Where(r => r.IsRun && r.Start >= Start && r.Start <= End).ToList();
                sportSummary.IsRun = true;
                GetPacePeakSummary(sportSummary);

            }
            else if (Sport == SportType.Swim)
            {
                sportActivitiesDuringPeriod = _summaryActivities.Where(r => r.IsSwim && r.Start >= Start && r.Start <= End).ToList();
                sportSummary.IsSwim = true;
            }
            else if (Sport == SportType.Other)
            {
                sportActivitiesDuringPeriod = _summaryActivities.Where(r => r.IsOther && r.Start >= Start && r.Start <= End).ToList();
                sportSummary.IsOther = true;
            }
            else
                sportActivitiesDuringPeriod = _summaryActivities.Where(r => r.Start >= Start && r.Start <= End).ToList();

            sportSummary.Sport = Sport;
            sportSummary.Duration = TimeSpan.FromSeconds(sportActivitiesDuringPeriod.Sum(r => r.MovingTime.TotalSeconds));
            sportSummary.Distance = sportActivitiesDuringPeriod.Sum(r => r.Distance);
            sportSummary.SufferScore = sportActivitiesDuringPeriod.Sum(r => r.SufferScore);
            sportSummary.Calories = sportActivitiesDuringPeriod.Sum(r => r.Calories);
            sportSummary.ElevationGain = sportActivitiesDuringPeriod.Sum(r => r.ElevationGain).ToFeet();
            sportSummary.ActivityCount = sportActivitiesDuringPeriod.Count();
            sportSummary.TSS = sportActivitiesDuringPeriod.Sum(r => r.TSS);
            sportSummary.TrainingLoadChartName = string.Format("{0}{1}Chart", Sport, (End - Start).TotalDays.ToString());

            return sportSummary;
        }

        private void GetPacePeakSummary(SportSummaryDto sportSummary)
        {
            var bestEfforts = _timesRepo.GetBestTimes(UserId, Start, End);

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

        private void GetPowerPeakSummary(SportSummaryDto sportSummary)
        {
            PeaksDtoRepository peaks = new PeaksDtoRepository(_context);
            peaks.UserId = UserId;
            PeaksDto peakDetails = peaks.ExtractPeaks(PeakStreamType.Power, Start, End);


            sportSummary.Peak1 = ExtractPeak(peakDetails, 5);
            sportSummary.Peak2 = ExtractPeak(peakDetails, 60);
            sportSummary.Peak3 = ExtractPeak(peakDetails, 1200);
            sportSummary.Peak4 = ExtractPeak(peakDetails, 3600);
        }

        private KeyValuePair<string, string> ExtractPeak(PeaksDto peaks, int duration)
        {
            foreach (var p in peaks.DurationPeaks)
            {
                if (p == null)
                    continue;

                if (p.Duration == duration)
                    return new KeyValuePair<string, string>(DisplayLabel.ShortStreamDurationForDisplay(duration),
                          string.Format("{0}{1}", p.Peak.ToString(), DisplayLabel.PeakStreamTypeUnits(PeakStreamType.Power)));
            }

            return new KeyValuePair<string, string>(DisplayLabel.ShortStreamDurationForDisplay(duration), "-");
        }
    }
}
