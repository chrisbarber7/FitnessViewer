using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessViewer.Infrastructure.Core.Models.Dto
{
    public class WeightByDayDto
    {
        public WeightByDayDto(DateTime date)
        {
            Date = date;
            Current = null;
        }

        public DateTime Date { get; set; }   
        public decimal? Current { get; set; }
        public decimal? Change7Day { get; set; }
        public decimal? Change30Day { get; set; }
        public decimal? Average7Day { get; set; }
        public decimal? Average30Day { get; set; }
        public decimal? Low7Day { get; set; }
        public decimal? Low30Day { get; set; }
        public decimal? High7Day { get; set; }
        public decimal? High30Day { get; set; }
    }
}
