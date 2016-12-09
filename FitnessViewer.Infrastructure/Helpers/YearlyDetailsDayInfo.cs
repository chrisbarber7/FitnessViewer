using FitnessViewer.Infrastructure.enums;
using System;

namespace FitnessViewer.Infrastructure.Helpers
{
    public class YearlyDetailsDayInfo
    {
        public YearlyDetailsDayInfo()
        {
            Distance = 0;
            YTDDistance = 0;
            Sequence = 0;
        }
        public DateTime Date { get; set; }
        public SportType Sport { get; set; }
        public decimal Distance { get; set; }
        public decimal YTDDistance { get; set; }
        public int Sequence { get; set; }
    }
}