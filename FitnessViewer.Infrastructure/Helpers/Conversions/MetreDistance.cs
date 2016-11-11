using System;

namespace FitnessViewer.Infrastructure.Helpers.Conversions
{
    /// <summary>
    /// Convert from metre distances (used by Strava) to user preferred units.
    /// </summary>
    public static class Distance
    {
        public const decimal METRE_TO_MILE = 0.00062137119M;
        public const decimal METER_TO_KM = 0.001M;
        public const decimal METER_TO_FEET = 3.2808399M;
        public const decimal METER_PER_SEC_TO_SECONDS_PER_MILE = 26.8224M;
        public const decimal METER_PER_SEC_TO_MILES_PER_HOUR = 2.236936M;
        public const int PRECISION = 2;

        /// <summary>
        /// Convert Metre distance to miles
        /// </summary>
        /// <param name="metres">metre distance to convert</param>
        /// <returns></returns>
        public static decimal MetersToMiles(decimal metres)
        {
            return Math.Round(metres * METRE_TO_MILE, PRECISION);
        }

        /// <summary>
        /// Convert Metre distance to KM
        /// </summary>
        /// <param name="metres">metre distance to convert</param>
        /// <returns></returns>
        public static decimal MetersToKilometers(decimal metres)
        {
            return Math.Round(metres * METER_TO_KM, PRECISION);
        }

        /// <summary>
        /// Convert Metre distance to feet
        /// </summary>
        /// <param name="metres">metre distance to convert</param>
        /// <returns></returns>
        public static decimal MetersToFeet(decimal metres)
        {
            return Math.Round(metres * METER_TO_FEET, PRECISION);
        }

        public static decimal MetersToFeet(int metres)
        {
            return Math.Round(metres * METER_TO_FEET, PRECISION);
        }

        /// <summary>
        /// Convert Metres per second (Stream.Velocity) to Seconds per mile
        /// </summary>
        /// <param name="metresPerSecond">Velocity (metres per second)</param>
        /// <returns></returns>
        public static int MetrePerSecondToSecondPerMile(decimal metresPerSecond)
        {
            return Convert.ToInt32(METER_PER_SEC_TO_SECONDS_PER_MILE / metresPerSecond * 60);
        }

        /// <summary>
        /// Convert Metres per second (Stream.Velocity) to Seconds per mile
        /// </summary>
        /// <param name="metresPerSecond">Velocity (metres per second)</param>
        /// <returns></returns>
        public static int MetrePerSecondToSecondPerMile(double metresPerSecond)
        {
            return MetrePerSecondToSecondPerMile(Convert.ToDecimal(metresPerSecond));
        }


        /// <summary>
        /// Convert Meters per second (Stream.Velocity) to Miles Per Hour
        /// </summary>
        /// <param name="metresPerSecond">Velocity (metres per second)</param>
        /// <returns>Miles Per Hour equivalent</returns>
        public static decimal MetrePerSecondToMilesPerHour(decimal metresPerSecond)
        {
            return METER_PER_SEC_TO_MILES_PER_HOUR * metresPerSecond;
        }
        /// <summary>
        /// Convert Meters per second (Stream.Velocity) to Miles Per Hour
        /// </summary>
        /// <param name="metresPerSecond">Velocity (metres per second)</param>
        /// <returns>Miles Per Hour equivalent</returns>
        public static decimal MetrePerSecondToMilesPerHour(double metresPerSecond)
        {
            return METER_PER_SEC_TO_MILES_PER_HOUR * Convert.ToDecimal(metresPerSecond);
        }

        /// <summary>
        /// Convert Meters per second (Stream.Velocity) to Miles Per Hour
        /// </summary>
        /// <param name="metresPerSecond">Velocity (metres per second)</param>
        /// <returns>Miles Per Hour equivalent</returns>
        public static decimal MetrePerSecondToMilesPerHour(int metresPerSecond)
        {
            return METER_PER_SEC_TO_MILES_PER_HOUR * metresPerSecond;
        }
    }
}
