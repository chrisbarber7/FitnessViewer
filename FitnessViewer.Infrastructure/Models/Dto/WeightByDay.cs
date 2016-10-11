using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessViewer.Infrastructure.Models.Dto
{
    public class WeightByDay
    {
        public WeightByDay(DateTime date)
        {
            Date = date;
            Current = null;
            Average7Day = null;
            Average30Day = null;
        }

        public DateTime Date { get; set; }   
        public decimal? Current { get; set; }
        public decimal? Average7Day { get; set; }
        public decimal? Average30Day { get; set; }
    }
}
