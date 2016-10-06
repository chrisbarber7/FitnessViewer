using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessViewer.Infrastructure.Models.Dto
{
    public class ActivitySummaryInformation
    {
        public ActivitySummaryInformation()
        {
            Power = new MinMaxAve();
            HeartRate = new MinMaxAve();
            Cadence = new MinMaxAve();
            Elevation = new MinMaxAve();
        }

        public TimeSpan Time { get; set; }
        public decimal? Distance { get; set; }
        public MinMaxAve Power { get; set; }
        public MinMaxAve HeartRate { get; set; }
        public MinMaxAve Cadence { get; set; }
        public MinMaxAve Elevation { get; set; }


        public class MinMaxAve
        {
            public int Min { get; set; }
            public int Max { get; set; }
            public int Ave { get; set; }
        }
    }
}
