using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessViewer.Infrastructure.Helpers
{
    public class PaceCalculator
    {
        /// <summary>
        /// Convert a distance/duration into minute pace per mile
        /// </summary>
        /// <param name="distanceInMetres">Run Distance (meters)</param>
        /// <param name="runDuration">Run Time</param>
        /// <returns></returns>
        public static TimeSpan RunMinuteMiles(decimal distanceInMetres, TimeSpan runDuration)
        {
            double distanceInMiles = Convert.ToDouble(distanceInMetres * MetreDistance.METRE_TO_MILE);

            if (distanceInMiles <= 0)
                return new TimeSpan(0, 0, 0);

            return TimeSpan.FromSeconds(Math.Round(runDuration.TotalSeconds / distanceInMiles));
        }
    }
}
