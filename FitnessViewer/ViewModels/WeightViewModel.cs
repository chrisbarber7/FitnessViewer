using System.ComponentModel.DataAnnotations;

namespace FitnessViewer.ViewModels
{
    public class WeightViewModel : MeasurementViewModel
    {
        public WeightViewModel() : base()
        { }

        [Required]
        [Range(1, 200, ErrorMessage = "Weight must be between 1 and 200kg")]
        [Display(Name = "Weight (kg)")]
        public decimal Weight { get; set; }


        [Range(0, 100, ErrorMessage = "Bodyfat must be between 0-100%.")]
        [Display(Name = "Bodyfat % (optional)")]
        public decimal? Bodyfat { get; set; }
    }
}