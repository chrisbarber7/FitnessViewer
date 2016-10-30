using FitnessViewer.Infrastructure.enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessViewer.Infrastructure.Models
{
    public class PeakStreamTypeDuration
    {
        [Key]
        [Column(Order = 1)]
        public PeakStreamType PeakStreamType { get; set; }

        [Key]
        [Column(Order = 2)]
        public int Duration { get; set; }
    }
}
