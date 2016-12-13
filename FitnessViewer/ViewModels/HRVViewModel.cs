using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FitnessViewer.ViewModels
{
    public class HRVViewModel : HeartRateViewModel
    {
        public HRVViewModel(DateTime defaultDate) : base(defaultDate)
        { }

        public HRVViewModel() : base()
        { }


        [Required]
        [Range(0, 100, ErrorMessage = "Heart Rate Variability must be between 1 and 100.")]
        [Display(Name = "Heart Rate Variability")]
        public decimal? HRV { get; set; }


        [Range(0, 10, ErrorMessage = "Readinesss must be between 0 and 10.")]
        [Display(Name = "Readiness Score")]
        public decimal? HRVReadiness { get; set; }

        [Range(0, 100, ErrorMessage = "RMSSD must be between 0 and 100.")]
        [Display(Name = "RMSSD (optional)")]
        public decimal? HRVRMSSD { get; set; }

        [Range(0, 100, ErrorMessage = "LnRMSSD must be between 0 and 100.")]
        [Display(Name = "LnRMSSD (optional)")]
        public decimal? HRVLnRMSSD { get; set; }

        [Range(0, 500, ErrorMessage = "SDNN must be between 0% and 500%")]
        [Display(Name = "SDNN (optional)")]
        public decimal? HRVSDNN { get; set; }

        [Range(0, 100, ErrorMessage = "NN50 must be between 0 and 100.")]
        [Display(Name = "NN50 (optional)")]
        public decimal? HRVNN50 { get; set; }

        [Range(0, 100, ErrorMessage = "PNN50 must be between 0 and 100.")]
        [Display(Name = "PNN50 (optional)")]
        public decimal? HRVPNN50 { get; set; }
    }
}