using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessViewer.Infrastructure.Models
{
    public class ActivityPeaks
    {
        public int Id { get; set; }

        [Required]
        [ForeignKey("Activity")]
        public long ActivityId { get; set; }
        public virtual Activity Activity { get; set; }

        public byte PeakType { get; set; }
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