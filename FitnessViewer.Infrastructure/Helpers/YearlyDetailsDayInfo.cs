using FitnessViewer.Infrastructure.enums;
using System;
using System.Collections;
using System.Collections.Generic;

namespace FitnessViewer.Infrastructure.Helpers
{
    public class YearlyDetailsDayInfo : IEqualityComparer<YearlyDetailsDayInfo>
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

        public bool Equals(YearlyDetailsDayInfo x, YearlyDetailsDayInfo y)
        {
            return x.Date == y.Date && x.Sport == y.Sport;
        }

        public int GetHashCode(YearlyDetailsDayInfo ytdInfo)
        {
            return ytdInfo.Date.GetHashCode() + ytdInfo.Sport.GetHashCode();
        }


    }
}