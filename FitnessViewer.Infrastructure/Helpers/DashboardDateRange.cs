using FitnessViewer.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessViewer.Infrastructure.Helpers
{
    public class DashboardDateRange
    {
        // for defaults how many days ago should we start?
        public const int DEFAULT_START_DAYS = -29;

        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        
        public AthleteSetting AthleteSetting { get; set; }
        public static DashboardDateRange CreateAndCalulcate(AthleteSetting setting)
        {
            DashboardDateRange range = new Helpers.DashboardDateRange(setting);
            range.Calculate();
            return range;
        }

        public static DashboardDateRange CreateAndCalulcate(string range, DateTime start, DateTime end)
        {
            AthleteSetting setting = new AthleteSetting();
            setting.DashboardRange = range;
            setting.DashboardStart = start;
            setting.DashboardEnd = end;

            return CreateAndCalulcate(setting);
        }

        private DashboardDateRange(AthleteSetting settings)
        {
            AthleteSetting = settings;
        }

        public void Calculate()
        {
            if (string.IsNullOrEmpty(AthleteSetting.DashboardRange))
            {
                if (AthleteSetting.DashboardStart == null ||
                    AthleteSetting.DashboardEnd == null || 
                    AthleteSetting.DashboardStart == DateTime.MinValue || 
                    AthleteSetting.DashboardEnd == DateTime.MinValue)
                {
                    SetDefaultDates();
                }
                else
                {
                    Start = AthleteSetting.DashboardStart.Date;
                    End = AthleteSetting.DashboardEnd.Date;
                }
            }
            else
            {
                switch (AthleteSetting.DashboardRange)
                {



                    case "Last 7 Days":
                        {
                            Start = DateTime.Now.AddDays(-6).Date;
                            End = DateTime.Now.Date;
                            break;
                        }
                    case "Last 30 Days":
                        {
                            Start = DateTime.Now.AddDays(-29).Date;
                            End = DateTime.Now.Date;
                            break;
                        }
                    case "Last 90 Days":
                        {
                            Start = DateTime.Now.AddDays(-89).Date;
                            End = DateTime.Now.Date;
                            break;
                        }
                    case "This Month":
                        {
                            Start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                            End = new DateTime(DateTime.Now.Year,
                                               DateTime.Now.Month,
                                               DateTime.DaysInMonth(DateTime.Now.Year,
                                                                    DateTime.Now.Month));
                            break;
                        }
                    case "Last Month":
                        {
                            Start = new DateTime(DateTime.Now.AddMonths(-1).Year, DateTime.Now.AddMonths(-1).Month, 1);
                            End = new DateTime(DateTime.Now.AddMonths(-1).Year,
                                               DateTime.Now.AddMonths(-1).Month,
                                               DateTime.DaysInMonth(DateTime.Now.AddMonths(-1).Year,
                                                                    DateTime.Now.AddMonths(-1).Month));

                            break;
                        }
                    case "This Year":
                        {
                            Start = new DateTime(DateTime.Now.Year, 1, 1);
                            End = DateTime.Now.Date;
                            break;
                        }
                    default:
                        {
                            SetDefaultDates();
                            break;
                        }
                }
            }
        }

        private void SetDefaultDates()
        {
            Start = DateTime.Now.AddDays(DEFAULT_START_DAYS).Date;
            End = DateTime.Now.Date;
        }
    }
}
