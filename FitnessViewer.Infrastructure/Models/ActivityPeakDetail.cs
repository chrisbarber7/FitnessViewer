using FitnessViewer.Infrastructure.enums;
using FitnessViewer.Infrastructure.Helpers;
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
            this.StartIndex = null;
            this.StreamType = type;
        }

        public int Id { get; set; }

        [Required]
        [ForeignKey("Activity")]
        [Index("IX_ActivityIdAndDuration", 1, IsUnique = true)]
        public long ActivityId { get; set; }
        public virtual Activity Activity { get; set; }

        [Index("IX_ActivityIdAndDuration", 3, IsUnique = true)]
        public int Duration { get; set; }       // duration of peak (in seconds)

        public int? Value { get; set; }         // peak

        private int? _startIndex;
        public int? StartIndex          // starting point in stream for the peak
        {
            get { return _startIndex; }
            set
            {
                _startIndex = value;
                if (_startIndex == null)
                    EndIndex = null;
                else
                {
                    EndIndex = _startIndex + Duration;
                }
            }
        }
        
        // ending point for the peak  Calculated from startIndex + duration.
        public int? EndIndex { get; private set; }

        [Index("IX_ActivityIdAndDuration", 2, IsUnique = true)]
        public PeakStreamType StreamType { get; set; }

        [NotMapped]
        public string DurationName
        {
            get
            {
                return StreamHelper.StreamDurationForDisplay(Convert.ToInt32(Duration));
            }
            private set { }
        }
    }
}
