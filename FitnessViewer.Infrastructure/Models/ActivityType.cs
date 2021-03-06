﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FitnessViewer.Infrastructure.Interfaces;
using FitnessViewer.Infrastructure.Data;

namespace FitnessViewer.Infrastructure.Models
{
    public class ActivityType : Entity<string>, IEntity<string>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override string Id { get; set; }

        public string Description { get; set; }
        public bool IsRide { get; set; }
        public bool IsRun { get; set; }
        public bool IsSwim { get; set; }
        public bool IsOther { get; set; }
    }
}


