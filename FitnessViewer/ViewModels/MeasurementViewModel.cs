using System;
using System.ComponentModel.DataAnnotations;

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
       
        public DateTime GetRecordedDateTime()
        {
            return DateTime.Parse(string.Format("{0} {1}", DateRecorded, TimeRecorded));
        }
    }
}