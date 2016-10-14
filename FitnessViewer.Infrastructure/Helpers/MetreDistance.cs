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
        public const decimal METRE_TO_MILE = 0.00062137119M;
        public const decimal METER_TO_KM = 0.001M;
        public const decimal METER_TO_FEET = 3.2808399M;
        public const int PRECISION = 2;

        /// <summary>
        /// Convert Metre distance to miles
        /// </summary>
        /// <param name="metres">metre distance to convert</param>
        /// <returns></returns>
        public static decimal ToMiles(decimal metres)
        {
            return Math.Round(metres * METRE_TO_MILE, PRECISION);
        }

        /// <summary>
        /// Convert Metre distance to KM
        /// </summary>
        /// <param name="metres">metre distance to convert</param>
        /// <returns></returns>
        public static decimal ToKM(decimal metres)
        {
            return Math.Round(metres * METER_TO_KM, PRECISION);
        }

        /// <summary>
        /// Convert Metre distance to feet
        /// </summary>
        /// <param name="metres">metre distance to convert</param>
        /// <returns></returns>
        public static decimal ToFeet(decimal metres)
        {
            return Math.Round(metres * METER_TO_FEET, PRECISION);
        }
    }
}
