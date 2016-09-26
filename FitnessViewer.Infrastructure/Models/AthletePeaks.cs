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
 

        public AthletePeaks()
        {
            Seconds5 = new AthletePeaksDetails();
            Minute1 = new AthletePeaksDetails();
            Minute5 = new AthletePeaksDetails();
            Minute20 = new AthletePeaksDetails();
            Minute60 = new AthletePeaksDetails();

        }

        public PeakStreamType PeakType { get; set; }
        public int Days { get; set; }        

        public AthletePeaksDetails Seconds5 { get; set; }
        public AthletePeaksDetails Minute1 { get; set; }
        public AthletePeaksDetails Minute5 { get; set; }
        public AthletePeaksDetails Minute20 { get; set; }
        public AthletePeaksDetails Minute60 { get; set; }

        public class AthletePeaksDetails {
            public AthletePeaksDetails( )
            {
            }

            public int? Peak { get; set; }
            public long? ActivityId { get; set; }
            public string Description { get; set; }
        }    
    }
}
