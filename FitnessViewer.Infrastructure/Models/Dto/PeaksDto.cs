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
            DurationPeaks = new List<PeaksDtoDetail>();
        }

        public PeakStreamType PeakType { get; set; }
        public int Days { get; set; }

        public List<PeaksDtoDetail> DurationPeaks;

        public class PeaksDtoDetail {
            public PeaksDtoDetail(int duration )
            {
                Duration = duration;
            }

            public PeaksDtoDetail()
            {
            }

            /// <summary>
            /// Peak Value
            /// </summary>
            public int? Peak { get; set; }

            /// <summary>
            /// Activity Id
            /// </summary>
            public long? ActivityId { get; set; }

            /// <summary>
            /// Activity Description
            /// </summary>
            public string Description { get; set; }

            /// <summary>
            /// Duration (in seconds)
            /// </summary>
            public int Duration { get; set; }
        }    
    }
}
