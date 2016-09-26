using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FitnessViewer.ViewModels
{
    public class ActivityViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Distance { get; set; }
        public float AverageSpeed { get; set; }
        public float ElevationGain { get; set; }
        public string Date { get; set; }
        public DateTime StartDateLocal { get; set; }

public TimeSpan ElapsedTime { get; set; }
        public string ActivityTypeId { get; set; }

    }
}


