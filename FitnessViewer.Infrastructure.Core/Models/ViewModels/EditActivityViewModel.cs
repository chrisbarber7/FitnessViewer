using System.ComponentModel.DataAnnotations;

namespace FitnessViewer.Infrastructure.Core.Models.ViewModels
{
    public class EditActivityViewModel
    {
        public long Id { get; set; }

        [Required]
        [Display(Name = "Activity Name")]
        public string Name { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name ="Activity Type")]
        public string ActivityTypeId { get; set; }

    
        [Display(Name = "Private")]
        public bool IsPrivate { get; set; }

        [Display(Name = "Gear")]
        public string GearId { get; set; }

        [Display(Name = "Commute")]
        public bool IsCommute { get; set; }

    }
}