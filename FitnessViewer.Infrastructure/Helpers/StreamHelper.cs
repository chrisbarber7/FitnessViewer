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
            RecalculateSingleActivity(uow, activityId);
        }

        public static void CalculatePowerCurveForDuration(long activityId, int duration)
        {
            UnitOfWork uow = new UnitOfWork();

            var powerData = uow.Activity.GetStreamForActivity(activityId).OrderBy(s => s.Time).Select(s => s.Watts).ToList();

            if (powerData.Contains(null))
                return;

            PeakValueFinder f = new PeakValueFinder(powerData.Select(w => w.Value).ToList(), PeakStreamType.Power, activityId, true);
            
            ActivityPeakDetail peak = f.FindPeakForDuration(duration);

            if (peak.Value != null)
            {
                uow.Analysis.AddPeakDetail(peak);
                uow.Complete();
            }
        }

        public static void AddPowerCurveCalculationJobs(string userId, long activityId)
        {
            UnitOfWork uow = new UnitOfWork();

            var powerData = uow.Activity.GetStreamForActivity(activityId).OrderBy(s => s.Time).Select(s => s.Watts).ToList();

            if (powerData.Contains(null))
                return;

            PeakValueFinder f = new PeakValueFinder(powerData.Select(w=>w.Value).ToList(), PeakStreamType.Power, activityId, true);

            int[] durations = f.StandardDurations;
            foreach (int d in durations)
            {

                DownloadQueue job = DownloadQueue.CreateQueueJob(userId, enums.DownloadType.CalculateActivityStats, activityId, d);
                uow.Queue.AddQueueItem(job);
                uow.Complete();
                AzureWebJob.AddToAzureQueue(job.Id);
            }
        }


        public static void RecalculateSingleActivity(UnitOfWork uow ,long activityId)
        {
            Console.WriteLine("Recalculating Peaks");
            RecalculatePower(uow, activityId);
            RecalculateHeartRate(uow, activityId);
            RecalculateCadence(uow, activityId);
            uow.Complete();
        }

        private static void RecalculateCadence(UnitOfWork uow, long activityId)
        {
            var cadenceData = uow.Activity.GetStream().Where(s => s.ActivityId == activityId).OrderBy(s => s.Time).Select(s => s.Cadence);
            List<ActivityPeakDetail> cadencePeaks = PeakValueFinder.ExtractPeaksFromStream(activityId, cadenceData.ToList(), PeakStreamType.Cadence);
            if (cadencePeaks != null)
                uow.Analysis.AddPeaks(activityId, PeakStreamType.Cadence, cadencePeaks);
        }

        private static void RecalculateHeartRate(UnitOfWork uow, long activityId)
        {
            var hrData = uow.Activity.GetStream().Where(s => s.ActivityId == activityId).OrderBy(s => s.Time).Select(s => s.HeartRate);
            List<ActivityPeakDetail> hrPeaks = PeakValueFinder.ExtractPeaksFromStream(activityId, hrData.ToList(), PeakStreamType.HeartRate);
            if (hrPeaks != null)
                uow.Analysis.AddPeaks(activityId, PeakStreamType.HeartRate, hrPeaks);

        }

        private static void RecalculatePower(UnitOfWork uow, long activityId)
        {
            var powerData = uow.Activity.GetStreamForActivity(activityId).OrderBy(s => s.Time).Select(s => s.Watts).ToList();

            List<ActivityPeakDetail> powerPeaks = PeakValueFinder.ExtractPeaksFromStream(activityId, powerData, PeakStreamType.Power);
            if (powerPeaks != null)
                uow.Analysis.AddPeaks(activityId, PeakStreamType.Power, powerPeaks);

        }
    }
}
