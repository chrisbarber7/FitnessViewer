using FitnessViewer.Infrastructure.Core.Data;
using FitnessViewer.Infrastructure.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FitnessViewer.Infrastructure.Core.Models
{
    [Index(nameof(ActivityId), IsUnique = true, Name = "IX_Stream_ActivityId")]
    [Index(nameof(ActivityId), nameof(Time), IsUnique = true, Name = "IX_Stream_ActivityIdAndStream")]
    public class Stream :  Entity<int>, IEntity<int>, IActivityEntity
    {
     //  public int Id { get; set; }

        [Required]
        [ForeignKey("Activity")]
     
        
        public long ActivityId { get; set; }
        public virtual Activity Activity { get; set; }

        public int Time { get; set; }

        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public double? Distance { get; set; }
        public double? Altitude { get; set; }
        public double? Velocity { get; set; }
        public int? HeartRate { get; set; }
        public int? Cadence { get; set; }
        public int? Watts { get; set; }
        public int? Temperature { get; set; }
        public bool? Moving { get; set; }
        public double? Gradient { get; set; }
    }
}
