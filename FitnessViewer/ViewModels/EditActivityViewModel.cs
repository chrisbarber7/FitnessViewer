using System.ComponentModel.DataAnnotations;

namespace FitnessViewer.ViewModels
{
    public class EditActivityViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Activity Name")]
        public string Name { get; set; }
    }
}