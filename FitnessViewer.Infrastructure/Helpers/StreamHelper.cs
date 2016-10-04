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
        public void RecalculateStreamPeaks()
        {
            UnitOfWork uow = new UnitOfWork();


            System.Diagnostics.Debug.WriteLine("Recalc Power");
            var activitiesWithPower = uow.Activity.GetStream().Where(s => s.Watts != null).Select(s => s.ActivityId).Distinct().ToList();

            foreach (long activityId in activitiesWithPower)
            {
                var powerData = uow.Activity.GetStream().Where(s => s.ActivityId == activityId).OrderBy(s => s.Time).Select(s => s.Watts);

                List<ActivityPeakDetail> powerPeaks = PeakValueFinder.ExtractPeaksFromStream(activityId, powerData.ToList(), PeakStreamType.Power);
                if (powerPeaks != null)
                {
                    uow.Analysis.AddPeak(activityId, PeakStreamType.Power, powerPeaks);
                    uow.Complete();
                }
            }


            System.Diagnostics.Debug.WriteLine("Recalc Heart Rate");
            var activitiesWithHeartRate = uow.Activity.GetStream().Where(s => s.HeartRate != null).Select(s => s.ActivityId).Distinct().ToList();

            foreach (long activityId in activitiesWithPower)
            {
                var hrData = uow.Activity.GetStream().Where(s => s.ActivityId == activityId).OrderBy(s => s.Time).Select(s => s.HeartRate);
                List<ActivityPeakDetail> hrPeaks = PeakValueFinder.ExtractPeaksFromStream(activityId, hrData.ToList(), PeakStreamType.HeartRate);
                if (hrPeaks != null)
                {
                    uow.Analysis.AddPeak(activityId, PeakStreamType.Power, hrPeaks);
                    uow.Complete();
                }

            }

            System.Diagnostics.Debug.WriteLine("Recalc Cadence");
            var activitiesWithCadence = uow.Activity.GetStream().Where(s => s.Cadence != null).Select(s => s.ActivityId).Distinct().ToList();

            foreach (long activityId in activitiesWithPower)
            {
                var cadenceData = uow.Activity.GetStream().Where(s => s.ActivityId == activityId).OrderBy(s => s.Time).Select(s => s.Cadence);
                List<ActivityPeakDetail> cadencePeaks = PeakValueFinder.ExtractPeaksFromStream(activityId, cadenceData.ToList(), PeakStreamType.Power);
                if (cadencePeaks != null)
                {
                    uow.Analysis.AddPeak(activityId, PeakStreamType.Power, cadencePeaks);
                    uow.Complete();
                }

            }
        }
    }
}
