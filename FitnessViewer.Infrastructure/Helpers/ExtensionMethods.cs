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
    }
}

