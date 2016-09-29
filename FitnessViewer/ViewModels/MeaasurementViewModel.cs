using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FitnessViewer.ViewModels
{
    public class MeasurementViewModel
    {
        [Required]
        [ValidDate]
        [Display(Name = "Recorded Date")]
        public string DateRecorded { get; set; }

        [ValidTime]
        [Display(Name = "Recorded Time")]
        public string TimeRecorded { get; set; }

        [Required]
        [Range(0.0, 200, ErrorMessage = "Weight must be between 0 and 200kg")]
        [Display(Name = "Weight (kg)")]
        public decimal Weight { get; set; }

        [Required]
        [Range(0, 100, ErrorMessage = "Bodyfat must be between 0-100%.")]
        public decimal Bodyfat { get; set; }

        public DateTime GetRecordedDateTime()
        {
            return DateTime.Parse(string.Format("{0} {1}", DateRecorded, TimeRecorded));
        }
    }
}