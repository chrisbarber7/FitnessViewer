using FitnessViewer.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessViewer.Infrastructure.Models.Collections
{
    public class ActivityPeakDetails
    {
        private IEnumerable<ActivityPeakDetail> _containedPeaks { get; }
        private UnitOfWork _unitOfWork;

        public ActivityPeakDetails(IEnumerable<ActivityPeakDetail> peaks)
        {
            _containedPeaks = peaks;
            _unitOfWork = new UnitOfWork();

        }

        public ActivityPeakDetails()
        {
            _containedPeaks = new List<ActivityPeakDetail>();                
        }

        /// <summary>
        /// Default save is a complete save updating both ActivityPeaks and ActivityPeakDetails
        /// </summary>
        public void Save()
        {
            Save(true);
        }


        /// <summary>
        /// Save a series of peak durations.
        /// </summary>
        /// <param name="updateActivityPeaks">True = update both ActivityPeaks and ActivityPeakDetails.
        /// False = Update only ActivityPeakDetails (used to create power curve records).</param>
        public void Save(bool updateActivityPeaks)
        {
            if (_containedPeaks.Count() == 0)
                return;

            if (updateActivityPeaks)
            {
                _unitOfWork.Analysis.AddPeaks(_containedPeaks.ToList());
                _unitOfWork.Complete();
            }
            else
            {
                foreach (ActivityPeakDetail p in _containedPeaks)
                    _unitOfWork.Analysis.AddPeakDetail(p);

                _unitOfWork.Complete();

            }
        }

    
    }
}
