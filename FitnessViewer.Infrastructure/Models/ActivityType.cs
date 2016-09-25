using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessViewer.Infrastructure.Models
{
    public class ActivityType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; set; }

        public string Description { get; set; }
        public bool IsRide { get; set; }
        public bool IsRun { get; set; }
        public bool IsSwim { get; set; }
        public bool IsOther { get; set; }
    }
}


