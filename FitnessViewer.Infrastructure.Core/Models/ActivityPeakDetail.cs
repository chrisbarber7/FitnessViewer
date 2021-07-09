using FitnessViewer.Infrastructure.Core.Data;
using FitnessViewer.Infrastructure.Core.enums;
using FitnessViewer.Infrastructure.Core.Helpers;
using FitnessViewer.Infrastructure.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessViewer.Infrastructure.Core.Models
{
    /// <summary>
    /// holds detail of a single peak.
    /// </summary>
    public class ActivityPeakDetail : Entity<int>, IEntity<int>, IActivityEntity
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

        [Required]
        [ForeignKey("Activity")]
        public long ActivityId { get; set; }
        public virtual Activity Activity { get; set; }

        //[Index("IX_ActivityPeakDetail_DurationAndStreamType", 1)]
        public int Duration { get; set; }                                                // duration of peak (in seconds)

        //[Index("IX_ActivityPeakDetail_Value")]
        public int? Value { get; set; }                                                  // peak

        private int? _startIndex;
        public int? StartIndex                                                           // starting point in stream for the peak
        {
            get { return _startIndex; }
            set
            {
                _startIndex = value;
                if (_startIndex == null)
                    EndIndex = null;
                else
                {
                    EndIndex = _startIndex + Duration - 1;
                }
            }
        }
        
        
        public int? EndIndex { get; private set; }                                      // ending point for the peak  Calculated from startIndex + duration.  

        //[Index("IX_ActivityPeakDetail_DurationAndStreamType", 2)]
        public PeakStreamType StreamType { get; set; }

        [NotMapped]
        public string DurationName
        {
            get
            {
                return DisplayLabel.StreamDurationForDisplay(Convert.ToInt32(Duration));
            }
            private set { }
        }
    }

    /// <summary>
    /// Add a Queue collection to base class which will be used to hold values used to 
    /// calculate the average.
    /// </summary>
    [NotMapped]
    public class ActivityPeakDetailCalculator : ActivityPeakDetail
    {
        public Queue<int> rollingValues = new Queue<int>();

        public ActivityPeakDetailCalculator(long activityId, PeakStreamType type, int duration) : base(activityId, type, duration)
        {
        }
    }
}
