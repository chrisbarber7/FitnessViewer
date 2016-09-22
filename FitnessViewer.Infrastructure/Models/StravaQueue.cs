using FitnessViewer.Infrastructure.Data;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessViewer.Infrastructure.Models
{
    public class StravaQueue
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(128)]
        [ForeignKey("User")]
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        public DateTime Added { get; set; }
        public bool Processed { get; set; }
        public DateTime? ProcessedAt { get; set; }

        [ForeignKey("Activity")]
        public long? StravaActivityId { get; set; }
        public virtual StravaActivity Activity { get; set; }
    }
}

