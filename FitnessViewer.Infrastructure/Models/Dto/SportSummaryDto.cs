using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessViewer.Infrastructure.Models.Dto
{
    public class SportSummaryDto
    {
        public string Sport { get; set; }
        public TimeSpan Duration { get; set; }
        public decimal Distance { get; set; }
        public int SufferScore { get; set; }
        public decimal Calories { get; set; }
        public decimal ElevationGain { get; set; }
        public int ActivityCount { get; set; }
    }
}
