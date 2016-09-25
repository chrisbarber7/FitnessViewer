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
        public float Distance { get; set; }
        public float AverageSpeed { get; set; }
        public float ElevationGain { get; set; }
        public DateTime StartDateLocal { get; set; }
        public string ActivityTypeId { get; set; }

    }
}


