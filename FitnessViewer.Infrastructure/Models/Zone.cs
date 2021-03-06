﻿using FitnessViewer.Infrastructure.Data;
using FitnessViewer.Infrastructure.enums;
using FitnessViewer.Infrastructure.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessViewer.Infrastructure.Models
{
    public class Zone : Entity<int>, IEntity<int>, IUserEntity
    {
      //  public int Id { get; set; }

        [Required]
        [MaxLength(128)]
        [ForeignKey("User")]
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ZoneType ZoneType { get; set; }

        /// <summary>
        /// Date from which this zone starts.
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Power=Watts, Heart Rate=Bpm, Pace=Seconds Per Mile
        /// </summary>
        public int Value { get; set; }
    }
}
