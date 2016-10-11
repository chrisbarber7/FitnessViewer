using FitnessViewer.Infrastructure.Models;
using FitnessViewer.Infrastructure.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FitnessViewer.ViewModels
{
    public class AthleteViewModel
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public IEnumerable<AthletePeaks> PowerPeaks { get; set; }
        public IEnumerable<RunningTimes> RunningTime { get; internal set; }
        public WeightByDay CurrentWeight { get; set; }
    }
}