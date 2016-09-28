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
        private const float METRE_TO_MILE = 0.00062137119F;
        private const float METER_TO_KM = 0.001F;
        private const float METER_TO_FEET = 3.2808399F;
        private const int PRECISION = 2;

        /// <summary>
        /// Convert Metre distance to miles
        /// </summary>
        /// <param name="metres">metre distance to convert</param>
        /// <returns></returns>
        public static float ToMiles(float metres)
        {
            return (float)Math.Round(metres * METRE_TO_MILE, PRECISION);
        }

        /// <summary>
        /// Convert Metre distance to KM
        /// </summary>
        /// <param name="metres">metre distance to convert</param>
        /// <returns></returns>
        public static float ToKM(float metres)
        {
            return (float)Math.Round(metres * METER_TO_KM, PRECISION);
        }

        /// <summary>
        /// Convert Metre distance to feet
        /// </summary>
        /// <param name="metres">metre distance to convert</param>
        /// <returns></returns>
        public static float ToFeet(float metres)
        {
            return (float)Math.Round(metres * METER_TO_FEET, PRECISION);
        }
    }
}
