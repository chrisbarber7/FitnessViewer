using FitnessViewer.Infrastructure.Models;
using FitnessViewer.Infrastructure.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FitnessViewer.ViewModels
{
    public class AthleteViewModel
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public IEnumerable<PeaksDto> PowerPeaks { get; set; }
        public IEnumerable<RunningTimesDto> RunningTime { get; internal set; }
        public WeightByDayDto CurrentWeight { get; set; }
        public IEnumerable<ActivityBaseDto> RecentActivity { get; set; }

        public SportSummaryDto Run7Day { get; set; }
        public SportSummaryDto Run30Day { get; set; }

        public SportSummaryDto Bike7Day { get; set; }
        public SportSummaryDto Bike30Day { get; set; }

        public SportSummaryDto Swim7Day { get; set; }
        public SportSummaryDto Swim30Day { get; set; }

        public SportSummaryDto Other7Day { get; set; }
        public SportSummaryDto Other30Day { get; set; }

        public SportSummaryDto All7Day { get; set; }
        public SportSummaryDto All30Day { get; set; }

    }
}