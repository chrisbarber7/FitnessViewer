using FitnessViewer.Infrastructure.Data;
using FitnessViewer.Infrastructure.enums;
using FitnessViewer.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessViewer.Infrastructure.Models
{
    public class ActivityPeaks : Entity<int>, IEntity<int>, IActivityEntity
    {
        [Required]
        [ForeignKey("Activity")]
        [Index("IX_ActivityPeaks_ActivityIdAndStreamType", 1, IsUnique = true)]
        public long ActivityId { get; set; }
        public virtual Activity Activity { get; set; }

        [Index("IX_ActivityPeaks_ActivityIdAndStreamType", 2, IsUnique = true)]
        public PeakStreamType StreamType { get; set; }

        public int? Peak5 { get; set; }
        public int? Peak10 { get; set; }
        public int? Peak30 { get; set; }
        public int? Peak60 { get; set; }
        public int? Peak120 { get; set; }
        public int? Peak300 { get; set; }
        public int? Peak360 { get; set; }
        public int? Peak600 { get; set; }
        public int? Peak720 { get; set; }
        public int? Peak1200 { get; set; }
        public int? Peak1800 { get; set; }
        public int? Peak3600 { get; set; }
        public int? PeakDuration { get; set; }
    }
}