using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FitnessViewer.ViewModels
{
    public class MeasurementViewModel
    {
        public MeasurementViewModel()
        {
            DateRecorded = string.Format("{0} {1} {2}",
                            DateTime.Now.Day.ToString().PadLeft(2, '0'),
                            DateTime.Now.ToString("MMM"),
                            DateTime.Now.Year.ToString());

            TimeRecorded = "00:00";
        }

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

  
        [Range(0, 100, ErrorMessage = "Bodyfat must be between 0-100%.")]
        public decimal? Bodyfat { get; set; }

        public DateTime GetRecordedDateTime()
        {
            return DateTime.Parse(string.Format("{0} {1}", DateRecorded, TimeRecorded));
        }
    }
}