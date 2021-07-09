using FitnessViewer.Infrastructure.Core.Helpers;
using FitnessViewer.Infrastructure.Core.Models.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitnessViewer.Infrastructure.Core.enums;
using FitnessViewer.Infrastructure.Core.Helpers.Analytics;

namespace FitnessViewer.Infrastructure.Core.Models.Dto
{
    public class ActivityMinMaxDto 
    {
        private ActivityStreams _activityStreams;

        public ActivityMinMaxDto(ActivityStreams activityStreams) 
        {
            _activityStreams = activityStreams;
            StreamSummary = new List<MinMaxAve>();
        }

        public List<MinMaxAve> StreamSummary { get; private set; }
        public TimeSpan Time { get; private set; }
        public ActivityAnalytics Analytics {get; private set;}
        public string Label { get; set; }
        public string WattsPerKg { get; private set; }
        public string Distance { get; private set; }
        public string AverageSpeed { get; private set; }
        public string ElevationGain { get; private set; }
        public string ElevationLoss { get; private set; }

        public void Populate()
        {
            if (_activityStreams == null)
                return;

            Analytics = _activityStreams.GetAnalytics();
            StreamSummary = _activityStreams.GetStreamSummary();
            Distance = _activityStreams.GetDistance().ToString("N1");
            Time = _activityStreams.GetTime();
            WattsPerKg = _activityStreams.GetWattsPerKg().ToString();
            AverageSpeed = _activityStreams.GetAverageSpeed().ToString("N1");
            ElevationGain = _activityStreams.GetElevationGain().ToString("N0");
            ElevationLoss = _activityStreams.GetElevationLoss().ToString("N0");
        }
    }
}
