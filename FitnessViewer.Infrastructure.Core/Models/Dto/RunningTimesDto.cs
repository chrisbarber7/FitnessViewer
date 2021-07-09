using FitnessViewer.Infrastructure.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessViewer.Infrastructure.Core.Models.Dto
{
    public class RunningTimesDto
    {

        public string ActivityName { get; set; }
        public DateTime ActivityDate { get; set; }
        public string DistanceName { get; set; }
        public decimal Distance { get; set; }
        public TimeSpan Time { get; set; }
        public long ActivityId { get; set; }

        public TimeSpan AveragePace
        {
            get
            {
                return PaceCalculator.RunMinuteMiles(Distance, Time);
            }
            private set
            {
            }
        }
    }
}
