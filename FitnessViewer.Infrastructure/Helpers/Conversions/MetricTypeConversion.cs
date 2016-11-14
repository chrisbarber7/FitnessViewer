using Fitbit.Models;
using FitnessViewer.Infrastructure.enums;

namespace FitnessViewer.Infrastructure.Helpers
{

    /// <summary>
    /// Conversions between Fitbit and Fitness viewer measurement/metric types.
    /// </summary>
    public class MetricTypeConversion
    {
        /// <summary>
        /// Return a FitnessViewer.MetricType for a given fitbit resource type.
        /// </summary>
        /// <param name="type">Fitbit resource type</param>
        /// <returns></returns>
        public static MetricType FromFitBitType(TimeSeriesResourceType type)
        {
            switch (type)
            {
                case TimeSeriesResourceType.Weight: { return MetricType.Weight; }
                case TimeSeriesResourceType.Fat: { return MetricType.BodyFat; }
                case TimeSeriesResourceType.CaloriesIn: { return MetricType.CaloriesIn; }
                case TimeSeriesResourceType.MinutesAsleep: { return MetricType.SleepMinutes; }
                case TimeSeriesResourceType.TimeInBed: { return MetricType.TimeInBed; }
                case TimeSeriesResourceType.TimeEnteredBed: { return MetricType.TimeEnteredBed; }

                default: { return MetricType.Invalid; }
            }
        }
    }
}
