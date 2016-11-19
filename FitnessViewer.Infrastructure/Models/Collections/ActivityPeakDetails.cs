using FitnessViewer.Infrastructure.Data;
using FitnessViewer.Infrastructure.enums;
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


        public static ActivityPeakDetails LoadForActivity(long activityId)
        {
            UnitOfWork uow = new UnitOfWork();
            var peaks = uow.CRUDRepository.GetByActivityId<ActivityPeakDetail>(activityId)
                                          .OrderBy(a => a.StreamType)
                                          .ThenBy(a => a.Duration)
                                          .ToList();
            
            return new ActivityPeakDetails(peaks);
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
                AddPeaks(_containedPeaks.ToList());
                _unitOfWork.Complete();
            }
            else
            {
                foreach (ActivityPeakDetail p in _containedPeaks)
                    _unitOfWork.CRUDRepository.Update<ActivityPeakDetail>(p);

                _unitOfWork.Complete();

            }
        }


        public void AddPeakDetail(ActivityPeakDetail peak)
        {
            var existingPeakDetail =  _unitOfWork.CRUDRepository
                                                 .GetByActivityId<ActivityPeakDetail>(peak.ActivityId)
                                                 .Where(a=>  a.StreamType == peak.StreamType && a.Duration == peak.Duration)
                                                 .FirstOrDefault();

            if (existingPeakDetail != null)
            {
                existingPeakDetail.Value = peak.Value;
                _unitOfWork.CRUDRepository.Update<ActivityPeakDetail>(existingPeakDetail);

            }
            else
                _unitOfWork.CRUDRepository.Add<ActivityPeakDetail>(peak);
                  
        }


        public void AddPeaks(List<ActivityPeakDetail> peaks)
        {
            if ((peaks == null) || (peaks.Count == 0))
                return;

            long activityId = peaks[0].ActivityId;
            PeakStreamType type = peaks[0].StreamType;


            var existingPeaks = _unitOfWork.CRUDRepository.GetByActivityId<ActivityPeaks>(activityId).Where(a => a.StreamType == type).ToList();
            if (existingPeaks.Count > 0)
                _unitOfWork.CRUDRepository.DeleteRange<ActivityPeaks>(existingPeaks);

            var existingPeakDetail = _unitOfWork.CRUDRepository.GetByActivityId<ActivityPeakDetail>(activityId)
                .Where(a =>  a.StreamType == type).ToList();
            if (existingPeakDetail.Count > 0)
                _unitOfWork.CRUDRepository.DeleteRange<ActivityPeakDetail>(existingPeakDetail);
               
            ActivityPeaks stravaPeak = new ActivityPeaks() { ActivityId = activityId, StreamType = type };

            foreach (ActivityPeakDetail d in peaks)
            {
                switch (d.Duration)
                {
                    case 5: { stravaPeak.Peak5 = d.Value; break; }
                    case 10: { stravaPeak.Peak10 = d.Value; break; }
                    case 30: { stravaPeak.Peak30 = d.Value; break; }
                    case 60: { stravaPeak.Peak60 = d.Value; break; }
                    case 120: { stravaPeak.Peak120 = d.Value; break; }
                    case 300: { stravaPeak.Peak300 = d.Value; break; }
                    case 360: { stravaPeak.Peak360 = d.Value; break; }
                    case 600: { stravaPeak.Peak600 = d.Value; break; }
                    case 720: { stravaPeak.Peak720 = d.Value; break; }
                    case 1200: { stravaPeak.Peak1200 = d.Value; break; }
                    case 1800: { stravaPeak.Peak1800 = d.Value; break; }
                    case 3600: { stravaPeak.Peak3600 = d.Value; break; }
                    case int.MaxValue: { stravaPeak.PeakDuration = d.Value; break; }
                }

                _unitOfWork.CRUDRepository.Add<ActivityPeakDetail>(d);
            }

            _unitOfWork.CRUDRepository.Add<ActivityPeaks>(stravaPeak);
        }
    }
}
