using FitnessViewer.Infrastructure.enums;
using FitnessViewer.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FitnessViewer.Infrastructure.Models.Dto
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

                return StreamHelper.StreamTypeUnits(this.Type);

            }
            private set { }
        }

        public int? StartIndex { get; set; }
        public int? EndIndex { get; set; }
        public int? StreamStep { get; set; }
    }
}