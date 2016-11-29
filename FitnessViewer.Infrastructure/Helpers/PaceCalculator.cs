using FitnessViewer.Infrastructure.Helpers.Conversions;
using System;

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
            double distanceInMiles = Convert.ToDouble(distanceInMetres * Distance.METRE_TO_MILE);

            if (distanceInMiles <= 0)
                return new TimeSpan(0, 0, 0);

            return TimeSpan.FromSeconds(Math.Round(runDuration.TotalSeconds / distanceInMiles));
        }

        /// <summary>
        /// Convert distance/duraction in average speed in MPH
        /// </summary>
        /// <param name="distanceInMetres">Activity Distance (metres)</param>
        /// <param name="duration">Activity Duration</param>
        /// <returns></returns>
        public static string AverageSpeed(decimal distanceInMetres, TimeSpan duration)
        {
            double distanceInMiles = Convert.ToDouble(distanceInMetres * Distance.METRE_TO_MILE);

            if (duration.TotalSeconds <= 0)
                return "0";

            return Math.Round(distanceInMiles / duration.TotalSeconds * 60 * 60, 2).ToString();
        }
    }
}
