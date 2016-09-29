using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessViewer.Infrastructure.Models
{
    public class Measurement
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public DateTime Recorded { get; set; }
        public decimal Weight { get; set; }
        public decimal Bodyfat { get; set; }
    }
}




