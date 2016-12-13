using System;
using System.ComponentModel.DataAnnotations;

namespace FitnessViewer.ViewModels
{
    public class SleepViewModel : MeasurementViewModel
    {
        public SleepViewModel(DateTime defaultDate) : base(defaultDate)
        { }

        public SleepViewModel() : base()
        { }


        [Required]
        [Range(0, 24, ErrorMessage = "Hours must be between 0 and 24.")]
        [Display(Name = "Hours")]
        public int Hours { get; set; }


        [Required]
        [Range(0, 60, ErrorMessage = "Minutes must be between 0 and 60.")]
        [Display(Name = "Minutes")]
        public int Minutes { get; set; }

        public int GetSleepMinutes()
        {
            return (Hours * 60) + Minutes;
        }

    }
}