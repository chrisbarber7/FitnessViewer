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
            DurationPeaks = new List<PeaksDetailDto>();
        }

        public PeakStreamType PeakType { get; set; }
        public int Days { get; set; }

        public List<PeaksDetailDto> DurationPeaks;

    }
}
