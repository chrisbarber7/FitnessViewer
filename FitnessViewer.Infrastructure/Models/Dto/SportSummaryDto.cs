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
        public SportSummaryDto()
        {
            Peak1 = new KeyValuePair<string, string>();
            Peak2 = new KeyValuePair<string, string>();
            Peak3 = new KeyValuePair<string, string>();
            Peak4 = new KeyValuePair<string, string>();
        }

        public decimal Distance { get; set; }
        public TimeSpan Duration { get; set; }
        public string Sport { get; set; }
        public int SufferScore { get; set; }
        public decimal Calories { get; set; }
        public decimal ElevationGain { get; set; }
        public int ActivityCount { get; set; }


        public bool IsRide { get; set; }

        public bool IsRun { get; set; }
        public bool IsSwim { get; set; }
        public bool IsOther { get; set; }

        public KeyValuePair<string,string> Peak1 { get; set; }
        public KeyValuePair<string, string> Peak2 { get; set; }
        public KeyValuePair<string, string> Peak3 { get; set; }
        public KeyValuePair<string, string> Peak4 { get; set; }
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
                    return string.Format("{0}mi", Math.Round(Distance.ToMiles(), 0).ToString());

                if (Distance > 20)
                    return string.Format("{0}mi", Math.Round(Distance.ToMiles(), 1).ToString());

                return string.Format("{0}mi", Math.Round(Distance.ToMiles(), 2).ToString());
            }
            private set { }
        }

        public string ElevationGainLabel
        {
            get
            {
                return ElevationGain.ToString("N0");
            }
            private set { }
        }
    
    }
}
