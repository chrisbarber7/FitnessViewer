using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessViewer.Infrastructure.Models.Dto
{
    public class ActivityAnalyticsDto
    {
        public static ActivityAnalyticsDto RideCreateFromPowerStream(List<Stream> stream, decimal ftp)
        {
            Helpers.ActivityAnalytics calc = new Helpers.ActivityAnalytics(stream, ftp);

            ActivityAnalyticsDto a = new ActivityAnalyticsDto()
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

        internal static ActivityAnalyticsDto RunCreateFromPaceOrHeartRateStream(List<double?> velocity, List<int?> heartRate)
        {
            ActivityAnalyticsDto a = new ActivityAnalyticsDto()
            {
                TSS = 0,
                IF = 0,
                NP = 0
            };

            return a;
        }

        internal static ActivityAnalyticsDto SwimCreateFromPaceStream(List<double?> list)
        {
            ActivityAnalyticsDto a = new ActivityAnalyticsDto()
            {
                TSS = 0,
                IF = 0,
                NP = 0
            };

            return a;
        }

        internal static ActivityAnalyticsDto OtherUnknown()
        {
            ActivityAnalyticsDto a = new ActivityAnalyticsDto()
            {
                TSS = 0,
                IF = 0,
                NP = 0
            };

            return a;
        }

    }
}

