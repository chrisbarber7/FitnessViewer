using FitnessViewer.Infrastructure.Data;
using FitnessViewer.Infrastructure.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessViewer.Infrastructure.Models.Collections
{
    public class ActivityPeakDetails
    {
        private List<ActivityPeakDetail> _containedPeaks { get; }
        private UnitOfWork _unitOfWork;


        public static ActivityPeakDetails LoadForActivity (long activityId)
        {
            UnitOfWork uow = new UnitOfWork();
            return new ActivityPeakDetails(uow.Activity.GetActivityPeakDetails(activityId));
        }

        public ActivityPeakDetails(IEnumerable<ActivityPeakDetail> peaks)
        {
            _unitOfWork = new UnitOfWork();
            _containedPeaks = peaks.ToList();
        }

        public IEnumerable<PowerCurveDto> GetPowerCurve()
        {
            return _containedPeaks.Where(p => p.StreamType == enums.PeakStreamType.Power && p.Value.HasValue)
                .Select(p => new PowerCurveDto
                {
                    Duration = p.Duration,
                    Watts = p.Value.Value
                })
                .OrderBy(p => p.Duration)
                .ToList();
        }

        public List<ActivityPeakDetail> Peaks
        {
            get { return _containedPeaks; }
            private set { }
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
