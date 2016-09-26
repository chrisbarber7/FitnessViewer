using FitnessViewer.Infrastructure.Data;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessViewer.Infrastructure.Models
{
    public class DownloadQueue
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

        public long? ActivityId { get; set; }
        public bool? HasError { get; set; }
        
    }
}

