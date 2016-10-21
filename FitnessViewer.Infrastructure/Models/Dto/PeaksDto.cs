using FitnessViewer.Infrastructure.enums;
using FitnessViewer.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessViewer.Infrastructure.Models.Dto
{
    public class PeaksDto
    {
        public PeaksDto()
        {
            Seconds5 = new PeakDetail();
            Minute1 = new PeakDetail();
            Minute5 = new PeakDetail();
            Minute20 = new PeakDetail();
            Minute60 = new PeakDetail();
        }

        public PeakStreamType PeakType { get; set; }
        public int Days { get; set; }        

        public PeakDetail Seconds5 { get; set; }
        public PeakDetail Minute1 { get; set; }
        public PeakDetail Minute5 { get; set; }
        public PeakDetail Minute20 { get; set; }
        public PeakDetail Minute60 { get; set; }

        public class PeakDetail {
            public PeakDetail( )
            {
            }

            public int? Peak { get; set; }
            public long? ActivityId { get; set; }
            public string Description { get; set; }
        }    
    }
}
