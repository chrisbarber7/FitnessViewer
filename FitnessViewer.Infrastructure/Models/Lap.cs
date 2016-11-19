using FitnessViewer.Infrastructure.Data;
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
    public class Lap : Entity<long>, IEntity<long>, IActivityEntity
    {
       // public long Id { get; set; }

        [Required]
        [ForeignKey("Activity")]
        public long ActivityId { get; set; }
        public virtual Activity Activity { get; set; }

     //   [Required]
        [ForeignKey("Athlete")]
        public long AthleteId { get; set; }
        public virtual Athlete Athlete { get; set; }

        public int ResourceState { get; set; }
        public string Name { get; set; }
   
        public TimeSpan MovingTime { get; set; }
        public TimeSpan ElapsedTime { get; set; }
        public DateTime Start { get; set; }
        public DateTime StartLocal { get; set; }
        public decimal Distance { get; set; }
        public int StartIndex { get; set; }
        public int EndIndex { get; set; }
        public decimal TotalElevationGain { get; set; }
        public decimal AverageSpeed { get; set; }
        public decimal MaxSpeed { get; set; }
        public decimal AverageCadence { get; set; }
        public decimal AveragePower { get; set; }
        public decimal AverageHeartrate { get; set; }
        public decimal MaxHeartrate { get; set; }
        public int LapIndex { get; set; }
    }
}

