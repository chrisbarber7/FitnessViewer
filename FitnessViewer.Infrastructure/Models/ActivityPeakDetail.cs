using FitnessViewer.Infrastructure.enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessViewer.Infrastructure.Models
{
    /// <summary>
    /// holds detail of a single peak.
    /// </summary>
    public class ActivityPeakDetail
    {
        private ActivityPeakDetail()
        { }


        public ActivityPeakDetail(long activityId, PeakStreamType type)
        {
            this.ActivityId = activityId;
            this.StreamType = type;
        }

        public ActivityPeakDetail(long activityId, PeakStreamType type, int duration)
        {
            this.ActivityId = activityId;
            this.Duration = duration;
            this.Value = null;
            this.Start = null;
            this.StreamType = type;
        }

        public int Id { get; set; }

        [Required]
        [ForeignKey("Activity")]
        public long ActivityId { get; set; }
        public virtual Activity Activity { get; set; }

        public int Duration { get; set; }       // duration of peak (in seconds)
        public int? Value { get; set; }         // peak
        public int? Start { get; set; }         // starting point in stream for the peak
        public PeakStreamType StreamType { get; set; }
    }
}
