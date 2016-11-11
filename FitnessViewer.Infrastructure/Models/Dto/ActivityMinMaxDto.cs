using FitnessViewer.Infrastructure.Helpers;
using FitnessViewer.Infrastructure.Models.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitnessViewer.Infrastructure.enums;

namespace FitnessViewer.Infrastructure.Models.Dto
{
    public class ActivityMinMaxDto : ActivityDto
    {
        public List<MinMaxAve> StreamSummary { get; private set; }

        private ActivityStreams _activityStreams;
        public ActivityMinMaxDto(ActivityStreams activityStreams)
        {
            _activityStreams = activityStreams;
            StreamSummary = new List<MinMaxAve>();

        }

        public ActivityMinMaxDto()
        {
            StreamSummary = new List<MinMaxAve>();
        }

        public TimeSpan Time { get; set; }

        public MinMaxAve Power { get; set; }
        public MinMaxAve HeartRate { get; set; }
        public MinMaxAve Cadence { get; set; }
        public MinMaxAve Elevation { get; set; }

        public MinMaxAve Temperature { get; set; }
        public MinMaxAve Speed { get; set; }

        public ActivityAnalyticsDto Analytics {get;set;}

        public string Label { get; set; }

        public decimal? WattsPerKg { get; set; }

        public void Populate()
        {
            if (_activityStreams.Stream.Count == 0)
                return;

            Weight = _activityStreams.Activity.Weight;
            Analytics = CreateAnalytics();
            StreamSummary = CreateStreamInformation();
            Distance = CalculateDistance();
            Time = TimeSpan.FromSeconds(_activityStreams.Stream.Count());
        }


        /// <summary>
        /// Calculate distance based on start/end distances of stream.
        /// </summary>
        /// <returns></returns>
        private decimal CalculateDistance()
        {
            var startDetails = _activityStreams.Stream.First();
            var endDetails = _activityStreams.Stream.Last();

            if ((startDetails.Distance != null) && (endDetails.Distance != null))
                return Helpers.Conversions.Distance.MetersToMiles(Convert.ToDecimal(endDetails.Distance.Value - startDetails.Distance.Value));

            return 0.00M;
        }

        /// <summary>
        /// Which streams are valid and available on the activity
        /// </summary>
        /// <returns></returns>
        private List<MinMaxAve>  CreateStreamInformation()
        {
            List<MinMaxAve> streamInfo = new List<MinMaxAve>();

            StreamType activityTypeStreams = StreamTypeHelper.SportStreams(_activityStreams.Activity.ActivityType);

            foreach (StreamType t in Enum.GetValues(typeof(StreamType)))
            {
                if (!activityTypeStreams.HasFlag(t))
                    continue;

                MinMaxAve mma = _activityStreams.GetMinMaxAve(t);

                if (mma.HasStream)
                    streamInfo.Add(mma);
            }

            return streamInfo.OrderBy(s => s.Priority).ToList();
        }

        /// <summary>
        /// Create Activity Analytics based on the activity type.
        /// </summary>
        /// <returns>Analytics</returns>
        private ActivityAnalyticsDto CreateAnalytics()
        {
            if (_activityStreams.Activity.ActivityType.IsRide)
                return ActivityAnalyticsDto.RideCreateFromPowerStream(_activityStreams.Stream, 295);
            else if (_activityStreams.Activity.ActivityType.IsRun)
                return ActivityAnalyticsDto.RunCreateFromPaceOrHeartRateStream(_activityStreams.Stream.Select(w => w.Velocity).ToList(), _activityStreams.Stream.Select(w => w.HeartRate).ToList());
            else if (_activityStreams.Activity.ActivityType.IsSwim)
               return ActivityAnalyticsDto.SwimCreateFromPaceStream(_activityStreams.Stream.Select(w => w.Velocity).ToList());
            else
              return ActivityAnalyticsDto.OtherUnknown();
        }
    }
}
