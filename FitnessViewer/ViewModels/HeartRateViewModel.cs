using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FitnessViewer.ViewModels
{
    public class HeartRateViewModel : MeasurementViewModel
    {
        public HeartRateViewModel(DateTime defaultDate) : base(defaultDate)
        { }

        public HeartRateViewModel() : base()
        { }
        
        [Required]
        [Range(25, 300, ErrorMessage = "Resting Heart Rate must be between 25 and 300bpm")]
        [Display(Name = "Resting Heart Rate (bpm)")]
        public decimal RestingHeartRate { get; set; }
    }
}