using FitnessViewer.Infrastructure.Core.Data;
using FitnessViewer.Infrastructure.Core.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessViewer.Infrastructure.Core.Models
{
    public class BestEffort : Entity<int>, IEntity<int>, IActivityEntity
    {
       // public int Id { get; set; }
        
        [Required]
        [ForeignKey("Activity")]
        public long ActivityId { get; set; }
        public virtual Activity Activity { get; set; }
        
        public int ResourceState { get; set; }

        //[Index("IX_BestEffort_Name")]
        [MaxLength(20)]
        public string Name { get; set; }

 //       [Index("IX_BestEffort_ElapsedTime")]
        public TimeSpan ElapsedTime { get; set; }

        public TimeSpan MovingTime { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime StartDateLocal { get; set; }
        public decimal Distance { get; set; }
        public int StartIndex { get; set; }
        public int EndIndex { get; set; }        
    }
}
