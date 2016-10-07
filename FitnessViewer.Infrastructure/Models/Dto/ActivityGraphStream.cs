using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessViewer.Infrastructure.Models.Dto
{
    public class ActivityGraphStream
    {
        public ActivityGraphStream()
        {
            Time = new List<int>();
            Distance = new List<double?>();
            Altitude = new List<double?>();
            Velocity = new List<double?>();
            HeartRate = new List<int?>();
            Cadence = new List<int?>();
            Watts = new List<int?>();
        }


        public List<int> Time { get; private set; } 
        public List<double?> Distance { get; private set; }
        public List<double?> Altitude { get; private set; }
        public List<double?> Velocity { get; private set; }
        public List<int?> HeartRate { get; private set; }
        public List<int?> Cadence { get; private set; }
        public List<int?> Watts { get; private set; }
        //     public int? Temperature { get; set; }
        //     public bool? Moving { get; set; }
        //     public double? Gradient { get; set; }
    }
}
