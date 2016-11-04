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

        public void Save()
        {
            if (_containedPeaks.Count() == 0)
                return;

            _unitOfWork.Analysis.AddPeaks(_containedPeaks.ToList());
            _unitOfWork.Complete();
        }
    }
}
