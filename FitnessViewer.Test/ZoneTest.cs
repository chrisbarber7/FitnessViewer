using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using FitnessViewer.Infrastructure.Interfaces;
using FitnessViewer.Infrastructure.Models;
using FitnessViewer.Infrastructure.Helpers.Analytics;

namespace FitnessViewer.Test
{
    [TestClass]
    public class ZoneTest
    {

        [TestMethod]
        public void Zone_GetValueOnGivenDayUser1()
        {

            Mock<ISettingsRepository> mock = MockZones();

            ZoneValueOnDay value = new ZoneValueOnDay(mock.Object);
            Assert.IsNull(value.GetUserZoneValueOnGivenDate("USER1", Infrastructure.enums.ZoneType.BikePower, new DateTime(2009, 12, 31)),"Before Initial Date");
            Assert.IsNull(value.GetUserZoneValueOnGivenDate("USER1", Infrastructure.enums.ZoneType.BikeHeartRate, new DateTime(2015, 12, 31)),"No Heart Rates set-up");

            Assert.AreEqual(100, value.GetUserZoneValueOnGivenDate("USER1", Infrastructure.enums.ZoneType.BikePower, new DateTime(2015, 12, 31)),"Using initial date");
            Assert.AreEqual(105, value.GetUserZoneValueOnGivenDate("USER1", Infrastructure.enums.ZoneType.BikePower, new DateTime(2016, 1, 1)),"Start of Year data");
            Assert.AreEqual(105, value.GetUserZoneValueOnGivenDate("USER1", Infrastructure.enums.ZoneType.BikePower, new DateTime(2016, 1, 2)),"Within 1st month");
            Assert.AreEqual(105, value.GetUserZoneValueOnGivenDate("USER1", Infrastructure.enums.ZoneType.BikePower, new DateTime(2016, 1, 31)), "End of Month");
            Assert.AreEqual(110, value.GetUserZoneValueOnGivenDate("USER1", Infrastructure.enums.ZoneType.BikePower, new DateTime(2016, 2, 1)),"Begin of month 2");
            Assert.AreEqual(160, value.GetUserZoneValueOnGivenDate("USER1", Infrastructure.enums.ZoneType.BikePower, new DateTime(2017, 1, 1)),"After Last date");

        }

        [TestMethod]
        public void Zone_GetValueOnGivenDayUser2()
        {

            Mock<ISettingsRepository> mock = MockZones();

            ZoneValueOnDay value = new ZoneValueOnDay(mock.Object);
            Assert.IsNull(value.GetUserZoneValueOnGivenDate("USER2", Infrastructure.enums.ZoneType.BikePower, new DateTime(2009, 12, 31)), "Before Initial Date");
            Assert.IsNull(value.GetUserZoneValueOnGivenDate("USER2", Infrastructure.enums.ZoneType.BikeHeartRate, new DateTime(2015, 12, 31)), "No Heart Rates set-up");

            Assert.IsNull(value.GetUserZoneValueOnGivenDate("USER2", Infrastructure.enums.ZoneType.BikePower, new DateTime(2015, 12, 31)), "day before initial date");
            Assert.AreEqual(255, value.GetUserZoneValueOnGivenDate("USER2", Infrastructure.enums.ZoneType.BikePower, new DateTime(2016, 11, 1)), "Start of month");
            Assert.AreEqual(255, value.GetUserZoneValueOnGivenDate("USER2", Infrastructure.enums.ZoneType.BikePower, new DateTime(2016, 11, 2)), "Within  month");
            Assert.AreEqual(255, value.GetUserZoneValueOnGivenDate("USER2", Infrastructure.enums.ZoneType.BikePower, new DateTime(2016, 11, 30)), "End of Month");
            Assert.AreEqual(260, value.GetUserZoneValueOnGivenDate("USER2", Infrastructure.enums.ZoneType.BikePower, new DateTime(2016, 12, 1)), "Begin of month ");
            Assert.AreEqual(260, value.GetUserZoneValueOnGivenDate("USER2", Infrastructure.enums.ZoneType.BikePower, new DateTime(2017, 1, 1)), "After Last date");

        }

        private Mock<ISettingsRepository> MockZones()
        {
            var results1 = new List<Zone>();
            var results2 = new List<Zone>();

            results1.Add(new Zone() { Id = 100 , StartDate = new DateTime(2010, 1, 1), UserId = "USER1", Value = 100 , ZoneType = Infrastructure.enums.ZoneType.BikePower });
 
            // user1 gets ids 100-112 and power 105-160, user1 gets ids 200-212 and power 205-260, 
            for (int month = 1; month <= 12; month++)
            {
                results1.Add(new Zone() { Id = 100 + month, StartDate = new DateTime(2016, month, 1), UserId = "USER1", Value = 100 + (month * 5), ZoneType = Infrastructure.enums.ZoneType.BikePower });
                results2.Add(new Zone() { Id = 200 + month, StartDate = new DateTime(2016, month, 1), UserId = "USER2", Value = 200 + (month * 5), ZoneType = Infrastructure.enums.ZoneType.BikePower });
            }

            var mock = new Mock<ISettingsRepository>();

            mock.Setup(x => x.GetUserZones("USER1", Infrastructure.enums.ZoneType.BikePower)).Returns(results1);
            mock.Setup(x => x.GetUserZones("USER2", Infrastructure.enums.ZoneType.BikePower)).Returns(results2);
            
            return mock;
        }
    }
}
