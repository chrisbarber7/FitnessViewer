using FitnessViewer.Infrastructure.Core.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessViewer.Infrastructure.Core.Models.Dto
{
    public class TimeDistanceBySportDto
    {
        public SportType Sport { get; set; }
        public string SportLabel { get; set; }
        public decimal Distance { get; set; }
        public decimal Duration{ get; set; }
    }
}
