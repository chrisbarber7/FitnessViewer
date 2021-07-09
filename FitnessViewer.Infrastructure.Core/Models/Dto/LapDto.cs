using FitnessViewer.Infrastructure.Core.enums;
using FitnessViewer.Infrastructure.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FitnessViewer.Infrastructure.Core.Models.Dto
{
    public class LapDto
    {
        public long Id { get; set; }
        public PeakStreamType Type { get; set; }
        public bool Selected { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string Units
        {
            get
            {
                if (string.IsNullOrEmpty(Value))
                    return string.Empty;

                return DisplayLabel.PeakStreamTypeUnits(this.Type);

            }
            private set { }
        }

        public int? StartIndex { get; set; }
        public int? EndIndex { get; set; }
        public int? StreamStep { get; set; }
        public int? SteppedStartIndex { get; set; }
        public int? SteppedEndIndex { get; set; }

        // fields below are only added so that a fix can be applied to laps (see comments in ActivityRepository.GetLaps)
        public TimeSpan MovingTime { get; set; }
        public int LapIndex { get; set; }
    }
}