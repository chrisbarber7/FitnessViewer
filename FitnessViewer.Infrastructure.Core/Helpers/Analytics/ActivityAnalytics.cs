using FitnessViewer.Infrastructure.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessViewer.Infrastructure.Core.Helpers.Analytics
{
    public class ActivityAnalytics
    {
        public static ActivityAnalytics RideCreateFromPowerStream(List<Stream> stream, decimal ftp)
        {
            BikePower calc = new BikePower(stream, ftp);

            ActivityAnalytics a = new ActivityAnalytics()
            {
                TSS = Math.Round(calc.TSS(),0),
                IF = Math.Round(calc.IntensityFactor(),2),
                NP = Math.Round(calc.NP(),0)
            };

            return a;
        }


        public decimal TSS { get; set; }
        public decimal IF { get; set; }
        public decimal NP { get; set; }

        internal static ActivityAnalytics RunCreateFromPaceOrHeartRateStream(List<double?> velocity, List<int?> heartRate)
        {
            ActivityAnalytics a = new ActivityAnalytics()
            {
                TSS = 0,
                IF = 0,
                NP = 0
            };

            return a;
        }

        internal static ActivityAnalytics SwimCreateFromPaceStream(List<double?> list)
        {
            ActivityAnalytics a = new ActivityAnalytics()
            {
                TSS = 0,
                IF = 0,
                NP = 0
            };

            return a;
        }

        internal static ActivityAnalytics OtherUnknown()
        {
            ActivityAnalytics a = new ActivityAnalytics()
            {
                TSS = 0,
                IF = 0,
                NP = 0
            };

            return a;
        }


        internal static ActivityAnalytics EmptyStream()
        {
            ActivityAnalytics a = new ActivityAnalytics()
            {
                TSS = 0,
                IF = 0,
                NP = 0
            };

            return a;
        }

    }
}

