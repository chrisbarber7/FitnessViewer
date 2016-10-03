using FitnessViewer.Infrastructure.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FitnessViewer.Infrastructure.Models.Dto
{
    public class ActivityLap
    {
        public long Id { get; set; }
        public LapType Type {get;set;}
        public bool Selected { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string Units { get; set; }
    }
}