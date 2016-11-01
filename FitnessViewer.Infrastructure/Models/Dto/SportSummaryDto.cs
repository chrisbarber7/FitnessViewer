using FitnessViewer.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessViewer.Infrastructure.Models.Dto
{
    public class SportSummaryDto
    {
        public decimal Distance { get; set; }
        public TimeSpan Duration { get; set; }
        public string Sport { get; set; }
        public int SufferScore { get; set; }
        public decimal Calories { get; set; }
        public decimal ElevationGain { get; set; }
        public int ActivityCount { get; set; }


        /// <summary>
        /// Duration formatted for display
        /// </summary>
        public string DurationLabel
        {
            get
            {
                return string.Format("{0}:{1}", Math.Floor(Duration.TotalHours).ToString().PadLeft(2, '0'),
                                                Duration.Minutes.ToString().PadLeft(2, '0'));
            }
            private set { }
        }

        /// <summary>
        ///  Distance formatted for display
        /// </summary>
        public string DistanceLabel
        {
            get
            {
                if (Sport == "Swim")
                {
                    if (Distance >= 10000)
                        return string.Format("{0}km", Math.Round(Distance / 1000, 1).ToString());
                    else
                        return string.Format("{0}m", Math.Round(Distance, 0).ToString());
                }
                if (Distance > 100)
                    return Math.Round(Distance.ToMiles(), 0).ToString();

                if (Distance > 20)
                    return Math.Round(Distance.ToMiles(), 1).ToString();

                return Math.Round(Distance.ToMiles(), 2).ToString();
            }
            private set { }
        }
    }
}
