﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessViewer.Infrastructure.Models.Dto
{
    public class PeriodDto
    {
        public string Period { get; set; }
        public decimal TotalDistance { get; set; }
        public decimal MaximumDistance { get; set; }
        public int Number { get; set; }
        public string Label { get; set; }
    }
}
