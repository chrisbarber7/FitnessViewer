using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessViewer.Infrastructure.Models.Dto
{
    public class ActivityByPeriodDto
    {
        public string ActivityType { get; set; }
        public string Period { get; set; }
        public decimal TotalDistance { get; set; }
        public int Number { get; set; }
        public string Label { get; set; }
    }
}
