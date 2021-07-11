using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessViewer.Infrastructure.Core.Models
{
    /// <summary>
    /// Calendar table.  To be used for easy reporting of week/month/year stats
    /// </summary>
    [Table("Calendars")]
    public class Calendar
    {
        public Calendar(DateTime d)
        {
            Date = d;
            UpdateValuesForDate();
        }

        public Calendar() { }

        [Key]
        public DateTime Date { get; set; }

        public int Year { get; private set; }
        public int Month { get; private set; }

        /// <summary>
        /// Return Year Month in the format YYYYMM
        /// </summary>
        [Required]
        [MaxLength(6)]
       // [Index]
        public string YearMonth { get; private set; }

        /// <summary>
        /// Return Year Week in the format YYYYWW
        /// </summary>
        [Required]
        [MaxLength(6)]
        //[Index]
        public string YearWeek { get; private set; }

        [Required]
        [MaxLength(9)]
        public string MonthName { get; private  set; }

        [Required]
        [MaxLength(9)]
        public string DayName { get; private set; }      

        [MaxLength(15)]
        public string WeekLabel { get; private set; }
       

        public void UpdateValuesForDate()
        {
            Year = Date.Year;
            Month = Date.Month;
            YearMonth = Year.ToString() + Month.ToString().PadLeft(2, '0');

            MonthName = Date.ToString("MMMM");
            DayName = Date.ToString("dddd");

            // week in year - YYYYWW
            int weekInYear = GetWeekNumber(Date);

            if (weekInYear >=52  && this.Month==1)
                this.YearWeek = (Year - 1).ToString() + weekInYear.ToString().PadLeft(2,'0');
            else if (weekInYear==1 && this.Month==12)
                this.YearWeek = (Year + 1).ToString() + weekInYear.ToString().PadLeft(2, '0');
            else
                this.YearWeek = (Year).ToString() + weekInYear.ToString().PadLeft(2, '0');

            // label for current week
            DateTime mondayOfWeek = MondayOfWeek(Date);
            DateTime sundayOfWeek = mondayOfWeek.AddDays(7);
       
            if (mondayOfWeek.Month == sundayOfWeek.Month)
                 WeekLabel = string.Format("{0}-{1} {2}", mondayOfWeek.Day.ToString(),
                                                        sundayOfWeek.Day.ToString(), 
                                                        mondayOfWeek.ToString("MMM"));
            else
                WeekLabel = string.Format("{0} {1}-{2} {3}", mondayOfWeek.Day.ToString(), 
                                                            mondayOfWeek.ToString("MMM"),
                                                            sundayOfWeek.Day.ToString(), 
                                                            sundayOfWeek.ToString("MMM"));
        }



        public static DateTime MondayOfWeek(DateTime dt)
        {
            int diff = dt.DayOfWeek - DayOfWeek.Monday;
            if (diff < 0)
            {
                diff += 7;
            }
            return dt.AddDays(-1 * diff).Date;
        }


        /// <summary>
        /// Calculate accurate week number with 7 days in each week.  Code originally taken from
        /// http://stackoverflow.com/questions/12196714/bug-in-weeknumber-calculation-net
        /// </summary>
        /// <param name="fromDate">Date for which to get the week number.</param>
        /// <returns>week number</returns>

        public static int GetWeekNumber(DateTime fromDate)
        {
            // Get jan 1st of the year
            DateTime startOfYear = fromDate.AddDays(-fromDate.Day + 1).AddMonths(-fromDate.Month + 1);
            // Get dec 31st of the year
            DateTime endOfYear = startOfYear.AddYears(1).AddDays(-1);
            // ISO 8601 weeks start with Monday 
            // The first week of a year includes the first Thursday 
            // DayOfWeek returns 0 for sunday up to 6 for saturday
            int[] iso8601Correction = { 6, 7, 8, 9, 10, 4, 5 };
            int nds = fromDate.Subtract(startOfYear).Days + iso8601Correction[(int)startOfYear.DayOfWeek];
            int wk = nds / 7;
            switch (wk)
            {
                case 0:
                    // Return weeknumber of dec 31st of the previous year
                    return Calendar.GetWeekNumber(startOfYear.AddDays(-1));
                case 53:
                    // If dec 31st falls before thursday it is week 01 of next year
                    if (endOfYear.DayOfWeek < DayOfWeek.Thursday)
                        return 1;
                    else
                        return wk;
                default: return wk;
            }
        }

    }


}
