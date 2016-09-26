using FitnessViewer.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessViewer.Infrastructure.Models
{
   public class AthletePeaks
    {
  
        public PeakStreamType PeakType { get; set; }
        public int Days { get; set; }
        public int? Peak05 { get; set; }
        public long? Peak05ActivityId { get; set; }
        public int? Peak60 { get; set; }
        public long? Peak60ActivityId { get; set; }
        public int? Peak300 { get; set; }
        public long? Peak300ActivityId { get; set; }
        public int? Peak1200 { get; set; }
        public long? Peak1200ActivityId { get; set; }
        public int? Peak3600 { get; set; }
        public long? Peak3600ActivityId { get; set; }
    }
}
