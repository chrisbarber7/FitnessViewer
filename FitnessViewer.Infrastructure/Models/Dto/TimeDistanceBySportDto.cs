using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessViewer.Infrastructure.Models.Dto
{
    public class TimeDistanceBySportDto
    {
        public string Sport { get; set; }
        public decimal Distance { get; set; }
        public decimal Duration{ get; set; }
    }
}
