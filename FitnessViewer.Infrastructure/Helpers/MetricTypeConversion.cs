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
                default: { return MetricType.Invalid; }
            }
        }
    }
}
