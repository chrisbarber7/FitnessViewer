using FitnessViewer.Infrastructure.Data;
using FitnessViewer.Infrastructure.enums;
using FitnessViewer.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessViewer.Infrastructure.Helpers
{
    public class StreamHelper
    {
        public static string StreamTypeUnits(PeakStreamType type)
        {
            switch(type)
                {
                case PeakStreamType.Cadence:  return "rpm";
                case PeakStreamType.HeartRate: return "bpm";
                case PeakStreamType.Lap: return "";
                case PeakStreamType.Power: return "watts";
                case PeakStreamType.Speed: return "mph";
                default: return "";
            }
        }

        public static string StreamDurationForDisplay(int duration)
        {
            if (duration == int.MaxValue)
                return "Activity";

            TimeSpan time = TimeSpan.FromSeconds(duration);

            if (time.Hours > 0)
                return time.ToString(@"hh\:mm\:ss");

            if (time.Minutes > 0 && time.Seconds > 0)
                return string.Format("{0} min {1} secs", time.Minutes.ToString(), time.Seconds.ToString());

            if (time.Minutes > 0 && time.Seconds == 0)
                return string.Format("{0} min", time.Minutes);

            return string.Format("{0} secs", duration.ToString());
        }

        public static void RecalculateSingleActivity(long activityId)
        {
            UnitOfWork uow = new UnitOfWork();
            RecalculatePower(uow, activityId);
            RecalculateHeartRate(uow, activityId);
            RecalculateCadence(uow, activityId);
        }


        public static void RecalculateAllActivities()
        {
            UnitOfWork uow = new UnitOfWork();

            System.Diagnostics.Debug.WriteLine("Recalc Power");
            var activitiesWithPower = uow.Activity.GetStream().Where(s => s.Watts != null).Select(s => s.ActivityId).Distinct().ToList();

            foreach (long activityId in activitiesWithPower)
                RecalculatePower(uow, activityId);

            System.Diagnostics.Debug.WriteLine("Recalc Heart Rate");
            var activitiesWithHeartRate = uow.Activity.GetStream().Where(s => s.HeartRate != null).Select(s => s.ActivityId).Distinct().ToList();

            foreach (long activityId in activitiesWithHeartRate)
                RecalculateHeartRate(uow, activityId);

            System.Diagnostics.Debug.WriteLine("Recalc Cadence");
            var activitiesWithCadence = uow.Activity.GetStream().Where(s => s.Cadence != null).Select(s => s.ActivityId).Distinct().ToList();

            foreach (long activityId in activitiesWithCadence)
                RecalculateCadence(uow, activityId);
        }

        private static void RecalculateCadence(UnitOfWork uow, long activityId)
        {
            var cadenceData = uow.Activity.GetStream().Where(s => s.ActivityId == activityId).OrderBy(s => s.Time).Select(s => s.Cadence);
            List<ActivityPeakDetail> cadencePeaks = PeakValueFinder.ExtractPeaksFromStream(activityId, cadenceData.ToList(), PeakStreamType.Cadence);
            if (cadencePeaks != null)
            {
                uow.Analysis.AddPeak(activityId, PeakStreamType.Cadence, cadencePeaks);
                uow.Complete();
            }
        }

        private static void RecalculateHeartRate(UnitOfWork uow, long activityId)
        {
            var hrData = uow.Activity.GetStream().Where(s => s.ActivityId == activityId).OrderBy(s => s.Time).Select(s => s.HeartRate);
            List<ActivityPeakDetail> hrPeaks = PeakValueFinder.ExtractPeaksFromStream(activityId, hrData.ToList(), PeakStreamType.HeartRate);
            if (hrPeaks != null)
            {
                uow.Analysis.AddPeak(activityId, PeakStreamType.HeartRate, hrPeaks);
                uow.Complete();
            }
        }

        private static void RecalculatePower(UnitOfWork uow, long activityId)
        {
            var powerData = uow.Activity.GetStreamForActivity(activityId).OrderBy(s => s.Time).Select(s => s.Watts).ToList();

            List<ActivityPeakDetail> powerPeaks = PeakValueFinder.ExtractPeaksFromStream(activityId, powerData, PeakStreamType.Power);
            if (powerPeaks != null)
            {
                uow.Analysis.AddPeak(activityId, PeakStreamType.Power, powerPeaks);
                uow.Complete();
            }
        }
    }
}
