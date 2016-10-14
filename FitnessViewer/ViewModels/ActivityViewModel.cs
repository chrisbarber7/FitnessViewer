using FitnessViewer.Infrastructure.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FitnessViewer.ViewModels
{
    public class ActivityViewModel
    {
        public ActivityViewModel()
        {
            Laps = new List<ActivityLapDto>();
        }

        public long Id { get; set; }
        public bool DetailsDownloaded { get; set; }
        public string Name { get; set; }
        public string Distance { get; set; }
        public decimal AverageSpeed { get; set; }
        public decimal ElevationGain { get; set; }
        public string Date { get; set; }
        public DateTime StartDateLocal { get; set; }

        public TimeSpan ElapsedTime { get; set; }
        public string ActivityTypeId { get; set; }

        public ActivitySummaryInformationDto SummaryInfo { get; set; }

        public IEnumerable<ActivityLapDto> Laps { get; set; }

        public IEnumerable<ActivityLapDto> Power { get; set; }
        public IEnumerable<ActivityLapDto> HeartRate { get; set; }
        public IEnumerable<ActivityLapDto> Cadence { get; set; }
    }
}


