using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessViewer.Infrastructure.Core.Models.Dto
{
    public class PeaksDetailDto
    {
        public PeaksDetailDto(int duration)
        {
            Duration = duration;
        }

        public PeaksDetailDto()
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
