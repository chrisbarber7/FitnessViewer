using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessViewer.Infrastructure.Models.Dto
{
    public class ActivityMinMaxDto : ActivityDto
    {
        public static ActivityMinMaxDto Create()
        {
            ActivityMinMaxDto a = new ActivityMinMaxDto();

            a.Power = new MinMaxAve();
            a.HeartRate = new MinMaxAve();
            a.Cadence = new MinMaxAve();
            a.Elevation = new MinMaxAve();

            return a;
        }

        public TimeSpan Time { get; set; }

        public MinMaxAve Power { get; set; }
        public MinMaxAve HeartRate { get; set; }
        public MinMaxAve Cadence { get; set; }
        public MinMaxAve Elevation { get; set; }

        public ActivityAnalyticsDto Analytics {get;set;}

        public class MinMaxAve
        {
            public int Min { get; set; }
            public int Max { get; set; }
            public int Ave { get; set; }
        }
    }
}
