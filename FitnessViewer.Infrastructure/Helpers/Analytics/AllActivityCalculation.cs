using FitnessViewer.Infrastructure.Data;
using FitnessViewer.Infrastructure.Models;
using FitnessViewer.Infrastructure.Models.Collections;
using System.Collections.Generic;
using System.Linq;

namespace FitnessViewer.Infrastructure.Helpers.Analytics
{
    /// <summary>
    /// Used to initially popualte ACTIVITY.TSS & ACTIVITY.IF columns. Re-use if formula changes by setting values 
    /// to null in DB then re-running
    /// </summary>
    public class AllActivityCalculation
    {

        private List<long> _activity;
        private UnitOfWork _uow;


        public AllActivityCalculation()
        {
            _uow = new UnitOfWork();

        }

        /// <summary>
        /// get a list of activities which don't have a TSS value but which do have a power meter so we can calculate TSS/IF
        /// </summary>
        private void GetActivities()
        {
            using (ApplicationDb db = new ApplicationDb())
            {
                _activity = db.Activity.Where(a => a.TSS == null && a.HasPowerMeter).Select(a => a.Id).ToList();
            }
            
        }

        /// <summary>
        /// Calculate Analytics for all activities
        /// </summary>
        public void CalculateAll()
        {
            GetActivities();

            if (_activity.Count == 0)
                return;

            // try populating TSS/IF values for each possible activity.That 
            foreach (long id in _activity)
            {
                Activity fvActivity = _uow.CRUDRepository.GetByKey<Activity>(id);

                if (fvActivity == null)
                    continue;

                CalculateActivity(fvActivity);
            }
        }

        private void CalculateActivity(Activity fvActivity)
        {
            ActivityStreams stream = ActivityStreams.CreateFromExistingActivityStream(fvActivity.Id);

            // stream must have watts to calculate TSS/IF!
            if (!stream.HasIndividualStream(enums.StreamType.Watts))
                return;

            BikePower calc = new BikePower(stream.GetIndividualStream<int?>(enums.StreamType.Watts), 295);

            fvActivity.TSS = calc.TSS();
            fvActivity.IntensityFactor = calc.IntensityFactor();

            _uow.Complete();
        }
    }
}
