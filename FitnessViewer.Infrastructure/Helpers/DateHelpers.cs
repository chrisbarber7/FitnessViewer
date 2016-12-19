using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessViewer.Infrastructure.Helpers
{
    public class DateHelpers
    {
        /// <summary>
        /// Convert Unix time format into DateTime type
        /// </summary>
        /// <param name="unixTimeStamp">Unix timestamp - seconds past 1/1/1970</param>
        /// <returns></returns>
        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        /// <summary>
        /// Convert DateTime to Unix timestamp.
        /// </summary>
        /// <param name="date">Date to convert</param>
        /// <returns>Unix timestamp</returns>
        public static int DateTimeToUnixTimeStamp(DateTime date)
        {
          return (int)(date.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        }

        /// <summary>
        /// To/From date details (input in unix format)
        /// </summary>
        public class DateRange
        {
            private double? _from;

            /// <summary>
            /// From/beginning of date range (Unix Format)
            /// </summary>
            public double? From { get { return _from; } set { _from = value; } }

            /// <summary>
            /// From/beginning of date range.
            /// </summary>
            public DateTime? FromDateTime
            {
                get
                {

                    if (_from == null)
                        return null;

                    return UnixTimeStampToDateTime(_from.Value).Date;
                }

                private set { }
            }

            private double? _to;

            /// <summary>
            /// To/end of date range (Unix Format)
            /// </summary>
            public double? To
            {
                get { return _to; }
                set { _to = value; }
            }    

            /// <summary>To/End of date range.
            /// 
            /// </summary>
            public DateTime? ToDateTime
            {
                get
                {
                    if (_to == null)
                        return null;

                    return UnixTimeStampToDateTime(_to.Value).Date;
                }

                private set { }
            }
        }
    }
}
