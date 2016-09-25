using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessViewer.Infrastructure.Helpers
{
    /// <summary>
    /// Convert from metre distances (used by Strava) to user preferred units.
    /// </summary>
    public static class MetreDistance
    {
        private const double METRE_TO_MILE = 0.00062137119;
        private const double METER_TO_KM = 0.001;
        private const double METER_TO_FEET = 3.2808399;
        private const int PRECISION = 2;

        /// <summary>
        /// Convert Metre distance to miles
        /// </summary>
        /// <param name="metres">metre distance to convert</param>
        /// <returns></returns>
        public static double ToMiles(double metres)
        {
            return Math.Round(metres * METRE_TO_MILE, PRECISION);
        }

        /// <summary>
        /// Convert Metre distance to KM
        /// </summary>
        /// <param name="metres">metre distance to convert</param>
        /// <returns></returns>
        public static double ToKM(double metres)
        {
            return Math.Round(metres * METER_TO_KM, PRECISION);
        }

        /// <summary>
        /// Convert Metre distance to feet
        /// </summary>
        /// <param name="metres">metre distance to convert</param>
        /// <returns></returns>
        public static double ToFeet(double metres)
        {
            return Math.Round(metres * METER_TO_FEET, PRECISION);
        }
    }
}
