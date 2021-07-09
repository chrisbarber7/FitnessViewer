using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessViewer.Infrastructure.Core.Models.Dto
{
    public class PeriodDto
    {
        public PeriodDto()
        {
            TotalDistance = 0;
            MaximumDistance = 0;
            Number = 0;
            PeriodAverageDistance = 0;
        }

        public string Period { get; set; }
        public decimal TotalDistance { get; set; }
        public decimal MaximumDistance { get; set; }
        public int Number { get; set; }
        public string Label { get; set; }
        public decimal PeriodAverageDistance { get; set; }
        public decimal PeriodAverageMaximumDistance { get; set; }
    }
}
