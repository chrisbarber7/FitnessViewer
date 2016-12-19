using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FitnessViewer.Infrastructure.Helpers;
using FitnessViewer.Infrastructure.Models;

namespace FitnessViewer.Test
{
    [TestClass]
    public class DashboardDateRangeTest
    {
        [TestMethod]
        public void DashboardDateRange_TestDefault()
        {
            AthleteSetting setting = new AthleteSetting();
            Assert.AreEqual(null, setting.DashboardRange);
          

            DashboardDateRange range = DashboardDateRange.CreateAndCalulcate(setting);

            // should use default date range.
            Assert.AreEqual(DateTime.Now.AddDays(DashboardDateRange.DEFAULT_START_DAYS).Date, range.Start);
            Assert.AreEqual(DateTime.Now.Date, range.End);
        }

        [TestMethod]
        public void DashboardDateRange_oneDateSet()
        {
            AthleteSetting setting = new AthleteSetting();
            setting.DashboardStart = DateTime.Now.AddDays(-1).Date;
            
            Assert.AreEqual(null, setting.DashboardRange);
            Assert.AreEqual(DateTime.Now.AddDays(-1).Date, setting.DashboardStart);
            

            DashboardDateRange range = DashboardDateRange.CreateAndCalulcate(setting);
            
            // just one date set should use default date range.
       
            Assert.AreEqual(DateTime.Now.AddDays(DashboardDateRange.DEFAULT_START_DAYS).Date, range.Start);
            Assert.AreEqual(DateTime.Now.Date, range.End);
        }

        [TestMethod]
        public void DashboardDateRange_BothDateSet()
        {
            AthleteSetting setting = new AthleteSetting();
            setting.DashboardStart = DateTime.Now.AddDays(-1).Date;
            setting.DashboardEnd = DateTime.Now.AddDays(1).Date;

          
            DashboardDateRange range = DashboardDateRange.CreateAndCalulcate(setting);

            // no range description set so should use dates provided.
            Assert.AreEqual(DateTime.Now.AddDays(-1).Date, range.Start);
            Assert.AreEqual(DateTime.Now.AddDays(1).Date, range.End);
        }


        [TestMethod]
        public void DashboardDateRange_InvalidRangeDescription()
        {
            string invalidRangeName = "Invalid Range";

            AthleteSetting setting = new AthleteSetting();
            setting.DashboardRange = invalidRangeName;
            setting.DashboardStart = DateTime.Now.AddDays(-1).Date;
            setting.DashboardEnd = DateTime.Now.AddDays(1).Date;

            Assert.AreEqual(invalidRangeName, setting.DashboardRange);

            DashboardDateRange range = DashboardDateRange.CreateAndCalulcate(setting);

            // invalid range deescription should use default ranges.
            Assert.AreEqual(DateTime.Now.AddDays(DashboardDateRange.DEFAULT_START_DAYS).Date, range.Start);
            Assert.AreEqual(DateTime.Now.Date, range.End);
        }

    

        [TestMethod]
        public void DashboardDateRange_Last7Days()
        {
            string rangeName = "Last 7 Days";

            AthleteSetting setting = new AthleteSetting();
            setting.DashboardRange = rangeName;

            Assert.AreEqual(rangeName, setting.DashboardRange);

            DashboardDateRange range = DashboardDateRange.CreateAndCalulcate(setting);

          DateTime  expectedStart = DateTime.Now.AddDays(-6).Date;
            DateTime expectedEnd = DateTime.Now.Date;

            Assert.AreEqual(expectedStart, range.Start);
            Assert.AreEqual(expectedEnd, range.End);

        }
        [TestMethod]
        public void DashboardDateRange_Last30Days()
        {
            string rangeName = "Last 30 Days";

            AthleteSetting setting = new AthleteSetting();
            setting.DashboardRange = rangeName;

            Assert.AreEqual(rangeName, setting.DashboardRange);

            DashboardDateRange range = DashboardDateRange.CreateAndCalulcate(setting);

            DateTime expectedStart = DateTime.Now.AddDays(-29).Date;
            DateTime expectedEnd = DateTime.Now.Date;

            Assert.AreEqual(expectedStart, range.Start);
            Assert.AreEqual(expectedEnd, range.End);
        }
        [TestMethod]
        public void DashboardDateRange_Last90Days()
        {
            string rangeName = "Last 90 Days";

            AthleteSetting setting = new AthleteSetting();
            setting.DashboardRange = rangeName;

            Assert.AreEqual(rangeName, setting.DashboardRange);

            DashboardDateRange range = DashboardDateRange.CreateAndCalulcate(setting);

            DateTime expectedStart = DateTime.Now.AddDays(-89).Date;
            DateTime expectedEnd = DateTime.Now.Date;

            Assert.AreEqual(expectedStart, range.Start);
            Assert.AreEqual(expectedEnd, range.End);
        }
        [TestMethod]
        public void DashboardDateRange_ThisMonth()
        {
            string rangeName = "This Month";

            AthleteSetting setting = new AthleteSetting();
            setting.DashboardRange = rangeName;

            Assert.AreEqual(rangeName, setting.DashboardRange);

            DashboardDateRange range = DashboardDateRange.CreateAndCalulcate(setting);

            DateTime expectedStart = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime expectedEnd = new DateTime(DateTime.Now.Year,
                               DateTime.Now.Month,
                               DateTime.DaysInMonth(DateTime.Now.Year,
                                                    DateTime.Now.Month));

            Assert.AreEqual(expectedStart, range.Start);
            Assert.AreEqual(expectedEnd, range.End);
        }
        [TestMethod]
        public void DashboardDateRange_LastMonth()
        {
            string rangeName = "Last Month";

            AthleteSetting setting = new AthleteSetting();
            setting.DashboardRange = rangeName;

            Assert.AreEqual(rangeName, setting.DashboardRange);

            DashboardDateRange range = DashboardDateRange.CreateAndCalulcate(setting);

            DateTime expectedStart = new DateTime(DateTime.Now.AddMonths(-1).Year, DateTime.Now.AddMonths(-1).Month, 1);
            DateTime expectedEnd = new DateTime(DateTime.Now.AddMonths(-1).Year,
                               DateTime.Now.AddMonths(-1).Month,
                               DateTime.DaysInMonth(DateTime.Now.AddMonths(-1).Year,
                                                    DateTime.Now.AddMonths(-1).Month));

            Assert.AreEqual(expectedStart, range.Start);
            Assert.AreEqual(expectedEnd, range.End);
        }
        [TestMethod]
        public void DashboardDateRange_ThisYear()
        {
            string rangeName = "This Year";

            AthleteSetting setting = new AthleteSetting();
            setting.DashboardRange = rangeName;

            Assert.AreEqual(rangeName, setting.DashboardRange);

            DashboardDateRange range = DashboardDateRange.CreateAndCalulcate(setting);

            DateTime expectedStart = new DateTime(DateTime.Now.Year, 1, 1);
            DateTime expectedEnd = DateTime.Now.Date;

            Assert.AreEqual(expectedStart, range.Start);
            Assert.AreEqual(expectedEnd, range.End);
        }


    }
}
