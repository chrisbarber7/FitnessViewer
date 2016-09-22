using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessViewer.Infrastructure.Models
{
    public class StravaBestEffort
    {
        public int Id { get; set; }
        
        [Required]
        [ForeignKey("Activity")]
        public long StravaActivityId { get; set; }
        public virtual StravaActivity Activity { get; set; }
        
        public int ResourceState { get; set; }
        public string Name { get; set; }
        public TimeSpan ElapsedTime { get; set; }
        public TimeSpan MovingTime { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime StartDateLocal { get; set; }
        public float Distance { get; set; }
        public int StartIndex { get; set; }
        public int EndIndex { get; set; }        
    }
}
