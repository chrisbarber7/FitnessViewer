using FitnessViewer.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessViewer.Infrastructure.Models.Dto
{
    public class ActivityDto
    {
        public static ActivityDto CreateFromActivity( Activity fvActivity)
        {
            ActivityDto m = new ActivityDto();

            m.Id = fvActivity.Id;
            m.Name = fvActivity.Name;
            m.ActivityTypeId = fvActivity.ActivityTypeId;
            m.DetailsDownloaded = true;
            m.Distance = fvActivity.Distance.ToMiles();
            m.AverageSpeed = 0;
            m.AveragePace = PaceCalculator.RunMinuteMiles(fvActivity.Distance, fvActivity.ElapsedTime.Value);
            m.ElevationGain = fvActivity.ElevationGain.ToFeet();
            m.Date = fvActivity.StartDateLocal.ToShortDateString();
            m.MovingTime = fvActivity.MovingTime.Value;
            m.Start = fvActivity.Start;
            m.StartDateLocal = fvActivity.StartDateLocal;
            m.SufferScore = fvActivity.SufferScore.HasValue ? fvActivity.SufferScore.Value : 0;
            m.Calories = fvActivity.Calories;
            m.HasMap = fvActivity.StartLatitude !=null ? true : false;
            m.IsRide = fvActivity.ActivityType.IsRide;
            m.IsRun = fvActivity.ActivityType.IsRun;
            m.IsSwim = fvActivity.ActivityType.IsSwim;
            m.IsOther = fvActivity.ActivityType.IsOther;

            m.HasPowerMeter = fvActivity.HasPowerMeter;

            m.Athlete = AthleteDto.CreateFromAthlete(fvActivity.Athlete);
            m.Weight = fvActivity.Weight;

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
        public DateTime Start { get; set; }
        public DateTime StartDateLocal { get; set; }

        public TimeSpan MovingTime { get; set; }

        public string ActivityTypeId { get; set; }
        public int SufferScore { get; set; }
        public decimal Calories { get; set; }
        public bool HasMap { get; set; }

        public bool IsRide { get; set; }
        public bool IsRun { get; set; }
        public bool IsSwim { get; set; }
        public bool IsOther { get; set; }
        public bool HasPowerMeter { get; set; }
        
        public AthleteDto Athlete { get; set; }
        public decimal? Weight { get; set; }

    }
}


