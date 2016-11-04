using FitnessViewer.Infrastructure.Models.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessViewer.Infrastructure.Models.Dto
{
    public class ActivityMinMaxDto : ActivityDto
    {




        public static ActivityMinMaxDto Create()
        {
            ActivityMinMaxDto a = new ActivityMinMaxDto();

            a.Power = new MinMaxAve();
            a.HeartRate = new MinMaxAve();
            a.Cadence = new MinMaxAve();
            a.Elevation = new MinMaxAve();

            return a;
        }

        public TimeSpan Time { get; set; }

        public MinMaxAve Power { get; set; }
        public MinMaxAve HeartRate { get; set; }
        public MinMaxAve Cadence { get; set; }
        public MinMaxAve Elevation { get; set; }

        public ActivityAnalyticsDto Analytics {get;set;}

        public class MinMaxAve
        {
            public int Min { get; set; }
            public int Max { get; set; }
            public int Ave { get; set; }
        }

        internal static ActivityMinMaxDto CreateFromActivityStreams(ActivityStreams activityStreams)
        {
            if (activityStreams.Stream.Count == 0)
                return new ActivityMinMaxDto();

            var startDetails = activityStreams.Stream.First();//.Where(s => s.Time == startIndex).First();
            var endDetails = activityStreams.Stream.Last(); // .Where(s => s.Time == endIndex).First();

            // need to group by something to get min/max/ave so grouping by a constant.
            var minMaxAveResults = activityStreams.Stream.GroupBy(i => 1)
          .Select(g => new
          {
              powerMin = g.Min(s => s.Watts),
              powerAve = g.Average(s => s.Watts),
              powerMax = g.Max(s => s.Watts),
              heartRateMin = g.Min(s => s.HeartRate),
              heartRateAve = g.Average(s => s.HeartRate),
              heartRateMax = g.Max(s => s.HeartRate),
              cadenceMin = g.Min(s => s.Cadence),
              cadenceAve = g.Average(s => s.Cadence),
              cadenceMax = g.Max(s => s.Cadence),
              elevationMin = g.Min(s => s.Altitude),
              elevationAve = g.Average(s => s.Altitude),
              elevationMax = g.Max(s => s.Altitude),
              time = g.Count(),
              distance = g.Sum(s => s.Distance)
          }).First();

            ActivityMinMaxDto info = ActivityMinMaxDto.Create();

            if (activityStreams.Activity.ActivityType.IsRide)
                info.Analytics = ActivityAnalyticsDto.RideCreateFromPowerStream(activityStreams.Stream, 295);
            else if (activityStreams.Activity.ActivityType.IsRun)
                info.Analytics = ActivityAnalyticsDto.RunCreateFromPaceOrHeartRateStream(activityStreams.Stream.Select(w => w.Velocity).ToList(), activityStreams.Stream.Select(w => w.HeartRate).ToList());
            else if (activityStreams.Activity.ActivityType.IsSwim)
                info.Analytics = ActivityAnalyticsDto.SwimCreateFromPaceStream(activityStreams.Stream.Select(w => w.Velocity).ToList());
            else
                info.Analytics = ActivityAnalyticsDto.OtherUnknown();

            if (minMaxAveResults.powerMin == null)
            {
                info.Power = null;
            }
            else
            {
                info.Power.Min = minMaxAveResults.powerMin.Value;
                info.Power.Max = minMaxAveResults.powerMax.Value;
                info.Power.Ave = Convert.ToInt32(minMaxAveResults.powerAve.Value);
            }

            if (minMaxAveResults.heartRateMin == null)
            {
                info.HeartRate = null;
            }
            else
            {
                info.HeartRate.Min = minMaxAveResults.heartRateMin.Value;
                info.HeartRate.Max = minMaxAveResults.heartRateMax.Value;
                info.HeartRate.Ave = Convert.ToInt32(minMaxAveResults.heartRateAve.Value);
            }

            if (minMaxAveResults.cadenceMin == null)
            {
                info.Cadence = null;
            }
            else
            {
                info.Cadence.Min = minMaxAveResults.cadenceMin.Value;
                info.Cadence.Max = minMaxAveResults.cadenceMax.Value;
                info.Cadence.Ave = Convert.ToInt32(minMaxAveResults.cadenceAve.Value);
            }

            if (minMaxAveResults.elevationMin == null)
            {
                info.Elevation = null;
            }
            else
            {
                info.Elevation.Min = Convert.ToInt32(minMaxAveResults.elevationMin.Value);
                info.Elevation.Ave = Convert.ToInt32(minMaxAveResults.elevationAve.Value);
                info.Elevation.Max = Convert.ToInt32(minMaxAveResults.elevationMax.Value);
            }

            if ((startDetails.Distance != null) && (endDetails.Distance != null))
                info.Distance = Helpers.Conversions.Distance.MetersToMiles(Convert.ToDecimal(endDetails.Distance.Value - startDetails.Distance.Value));

            info.Time = TimeSpan.FromSeconds(activityStreams.Stream.Count());



            return info;

        }
    }
}
