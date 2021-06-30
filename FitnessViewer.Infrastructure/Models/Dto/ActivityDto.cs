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
            m.TSS = fvActivity.TSS.HasValue ? fvActivity.TSS.Value : 0;

            m.Athlete = AthleteDto.CreateFromAthlete(fvActivity.Athlete);
            m.Weight = fvActivity.Weight;
            m.TSS = fvActivity.TSS;

            return m;
        }

        public string AveragePace{
            get
            {
                if (IsRun)
                {
                    return PaceCalculator.RunMinuteMiles(Distance, MovingTime).ToMinSec();
                }
                else if (IsSwim)
                {
                    return "";
                }
                else
                {
                    return "";

                }

            }
        private set { }
        }

        public string AverageSpeed
        {
            get
            {
                if ((IsRide) || (IsOther))
                {
                    var averageSpeed = PaceCalculator.AverageSpeed(Distance, MovingTime);
                    return averageSpeed.ToString();
                }
                
                return "";
            }
            private set { }
        }
        public long Id { get; set; }
        public bool DetailsDownloaded { get; set; }
        public string Name { get; set; }
        public decimal Distance { get; set; }
        public decimal ElevationGain { get; set; }
        public string Date { get; set; }
        public DateTime Start { get; set; }
        public DateTime StartDateLocal { get; set; }

        public TimeSpan MovingTime { get; set; }

        public string ActivityTypeId { get; set; }
        public decimal SufferScore { get; set; }
        public decimal Calories { get; set; }
        public bool HasMap { get; set; }

        public bool IsRide { get; set; }
        public bool IsRun { get; set; }
        public bool IsSwim { get; set; }
        public bool IsOther { get; set; }
        public bool HasPowerMeter { get; set; }
        
        public AthleteDto Athlete { get; set; }
        public decimal? Weight { get; set; }

        public decimal? TSS { get; set; }

    }
}


