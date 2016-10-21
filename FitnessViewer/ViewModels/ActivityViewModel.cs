using FitnessViewer.Infrastructure.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FitnessViewer.Infrastructure.Models;
using FitnessViewer.Infrastructure.Helpers;
using FitnessViewer.Infrastructure.enums;
using FitnessViewer.Infrastructure.Data;

namespace FitnessViewer.ViewModels
{
    public class ActivityViewModel
    {
   
        public ActivityViewModel()
        {
            Laps = new List<LapDto>();
        }

        public static ActivityViewModel CreateFromActivity(UnitOfWork uow, Activity fvActivity)
        {
            ActivityViewModel m = new ActivityViewModel();

            m.Id = fvActivity.Id;
            m.Name = fvActivity.Name;
            m.ActivityTypeId = fvActivity.ActivityTypeId;
            m.DetailsDownloaded = true;
            m.Distance = fvActivity.ActivityType.IsSwim ? fvActivity.Distance : MetreDistance.ToMiles(fvActivity.Distance);
            m.AverageSpeed = 0;
            m.AveragePace = PaceCalculator.RunMinuteMiles(fvActivity.Distance, fvActivity.ElapsedTime.Value);
            m.ElevationGain = MetreDistance.ToFeet(fvActivity.ElevationGain);
            m.Date = fvActivity.StartDateLocal.ToShortDateString();
            m.MovingTime = fvActivity.MovingTime.Value;
            m.Laps = uow.Activity.GetLaps(fvActivity.Id);
            m.Power = uow.Activity.GetLapStream(fvActivity.Id, PeakStreamType.Power);
            m.HeartRate = uow.Activity.GetLapStream(fvActivity.Id, PeakStreamType.HeartRate);
            m.Cadence = uow.Activity.GetLapStream(fvActivity.Id, PeakStreamType.Cadence);
            m.SummaryInfo = uow.Activity.BuildSummaryInformation(fvActivity.Id, 0, int.MaxValue);
            return m;
        }

        public long Id { get; set; }
        public bool DetailsDownloaded { get; set; }
        public string Name { get; set; }
        public decimal Distance { get; set; }
        public decimal AverageSpeed { get; set; }
        public TimeSpan AveragePace { get; set; }
        public decimal ElevationGain { get; set; }
        public string Date { get; set; }
        public DateTime StartDateLocal { get; set; }

        public TimeSpan MovingTime { get; set; }

        public string ActivityTypeId { get; set; }

        public MinMaxDto SummaryInfo { get; set; }
        public IEnumerable<LapDto> Laps { get; set; }
        public IEnumerable<LapDto> Power { get; set; }
        public IEnumerable<LapDto> HeartRate { get; set; }
        public IEnumerable<LapDto> Cadence { get; set; }
    }
}


