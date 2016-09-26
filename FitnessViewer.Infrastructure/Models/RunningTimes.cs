using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessViewer.Infrastructure.Models
{
    public class RunningTimes
    {
     
            public string ActivityName { get; set; }
            public DateTime ActivityDate { get; set; }
            public string DistanceName { get; set; }
            public float Distance { get; set; }
            public TimeSpan Time { get; set; }
            public long ActivityId { get; set; }
          
        }
    }
