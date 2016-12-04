using FitnessViewer.Infrastructure.Helpers.Conversions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessViewer.Infrastructure.Helpers
{
   public static class ExtensionMethods
    {
        /// <summary>
        /// Convert a distance in meters to a distance in miles.
        /// </summary>
        /// <param name="distanceInMetres">Distance to convert (in meters)</param>
        /// <returns>Distance in Miles</returns>
        public static decimal ToMiles(this decimal distanceInMetres)
        {
            return Distance.MetersToMiles(distanceInMetres);
        }

        /// <summary>
        /// Convert a distance in meters to a distance in feet.
        /// </summary>
        /// <param name="distanceInMetres">Distance to convert (in meters)</param>
        /// <returns>Distance in Miles</returns>
        public static decimal ToFeet(this decimal distanceInMetres)
        {
            return Distance.MetersToFeet(distanceInMetres);
        }

        /// <summary>
        /// Convert a distance in meters to a distance in feet.
        /// </summary>
        /// <param name="distanceInMetres">Distance to convert (in meters)</param>
        /// <returns>Distance in Miles</returns>
        public static decimal ToFeet(this int distanceInMetres)
        {
            return Distance.MetersToFeet(distanceInMetres);
        }

        /// <summary>
        /// Convert a distance in meters to a distance in feet.
        /// </summary>
        /// <param name="distanceInMetres">Distance to convert (in meters)</param>
        /// <returns>Distance in Miles</returns>
        public static decimal ToFeet(this double distanceInMetres)
        {
            return Distance.MetersToFeet(Convert.ToDecimal(distanceInMetres));
        }

        /// <summary>
        ///  Format TimeSpan/Duration as MM:SS
        /// </summary>
        /// <param name="duration">Time</param>
        /// <returns>Time formatted as MM:SS</returns>
        public static string ToMinSec(this TimeSpan duration)
        {
            return string.Format("{0}:{1}", duration.Minutes.ToString().PadLeft(2, '0'), duration.Seconds.ToString().PadLeft(2, '0'));
        }
    }
}

