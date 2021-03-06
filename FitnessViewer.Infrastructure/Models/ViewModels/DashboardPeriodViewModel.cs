﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FitnessViewer.Infrastructure.Helpers.DateHelpers;

namespace FitnessViewer.Infrastructure.Models.ViewModels
{
    public class DashboardPeriodViewModel : DateRange
    {
        public string UserId { get; set; }

        public DateTime DashboardStart { get; set; }
        public DateTime DashboardEnd { get; set; }
        public string DashboardRange { get; set; }
    }
}
