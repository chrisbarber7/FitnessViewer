using FitnessViewer.Infrastructure.enums;
using FitnessViewer.Infrastructure.Helpers;
using FitnessViewer.Infrastructure.Intefaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessViewer.Test
{


    [TestClass]
    public class YearlyDetailsTest
    {

        private const string USER_ID = "uid";

        [TestMethod]
        public void BikeMayData()
            {

            Mock<IActivityDtoRepository> mock = Ride2016DataWithDataForMay();

            YearlyDetails ytd = new YearlyDetails(mock.Object);

            ytd.Populate(USER_ID, null);
            ytd.Calculate();

            // each day in May has a distance so max sequence should equal days in the May (31).
            Assert.AreEqual(31, ytd.MaxSequence(SportType.Ride, 2016));

            // Test data has rows for each day in 2016.  Disance is zero for all days bar May.  May has a distance equal to the day
            // of the month.  
            decimal totalDistance = 0;
            for (int day = 0; day >= 31; day++)
            {
                YearlyDetailsDayInfo i = ytd.DayInformation.Where(d => d.Date == new DateTime(2016, 5, day)).FirstOrDefault();

                // day should exist in the collection.
                Assert.IsNotNull(i);

                // total distance for May should be sum of the days of month so far. eg. 5th = 1+2+3+4+5.
                Assert.AreEqual(totalDistance, i.Distance);

                // as each day in May has a distance sequence should equal day of the month.
                Assert.AreEqual(day, i.Sequence);

                totalDistance += day;
            }
        }

        [TestMethod]
        public void DailyRidingTest()
        {
            Mock<IActivityDtoRepository> mock = FiveYearDataWithDailyRiding();

            YearlyDetails ytd = new YearlyDetails(mock.Object);

            ytd.Populate(USER_ID, null);
            ytd.Calculate();


            Assert.AreEqual(1000, ytd.DailyAverage(SportType.Ride, null));
            Assert.AreEqual(250, ytd.DailyAverage(SportType.All, null));
            Assert.AreEqual(0, ytd.DailyAverage(SportType.Run, null));

            Assert.AreEqual(1000, ytd.DailyAverage(SportType.Ride, 2016));
            Assert.AreEqual(250, ytd.DailyAverage(SportType.All, 2016));
            Assert.AreEqual(0, ytd.DailyAverage(SportType.Run, 2016));

            Assert.AreEqual(0, ytd.DailyAverage(SportType.Ride, 2017));
            Assert.AreEqual(0, ytd.DailyAverage(SportType.All, 2017));
            Assert.AreEqual(0, ytd.DailyAverage(SportType.Run, 2017));
        }

        [TestMethod]
        public void CheckMissingDaysAdded()
        {
            Mock<IActivityDtoRepository> mock = DataWithGapInDays();

            YearlyDetails ytd = new YearlyDetails(mock.Object);

            ytd.Populate(USER_ID, null);

            // should be one per day of year for each of the 4 sporst        Assert.AreEqual(0, trueData2);
            Assert.AreEqual(366*4, ytd.DayInformation.Count());

            // test for first of days with activity data.
            var trueData1 = ytd.DayInformation
                                .Where(a => a.Date == new DateTime(2016, 2, 1))
                                .Where(a => a.Sport == SportType.Ride)
                                .Select(a => a.Distance)
                                .FirstOrDefault();

            Assert.AreEqual(1000, trueData1);

            // test for second of days with activity data.
            var trueData2 = ytd.DayInformation
                              .Where(a => a.Date == new DateTime(2016, 2, 1))
                              .Where(a => a.Sport == SportType.Ride)
                              .Select(a => a.Distance)
                              .FirstOrDefault();

            Assert.AreEqual(1000, trueData2);

            var addedData1 = ytd.DayInformation
                              .Where(a => a.Date == new DateTime(2016, 2, 2))
                              .Where(a => a.Sport == SportType.Ride)
                              .Select(a => a.Distance)
                              .FirstOrDefault();

            Assert.AreEqual(0, addedData1);
        }

        private Mock<IActivityDtoRepository> Ride2016DataWithDataForMay()
        {
            var results = new List<YearlyDetailsDayInfo>();

            for (DateTime d = new DateTime(2016, 1, 1); d <= new DateTime(2016, 12, 31); d = d.AddDays(1))
            {
                int distance = 0;

                // for May add 1000m * day of month for distance each day. 
                if (d.Month == 5)
                    distance = 1000;

                results.Add(new YearlyDetailsDayInfo() { Date = d, Sport = SportType.Ride, Distance=distance });
            }

            var mock = new Mock<IActivityDtoRepository>();

            mock.Setup(x => x.GetYearToDateInfo(USER_ID, null))
                .Returns(results);

            return mock;
        }


        private Mock<IActivityDtoRepository> FiveYearDataWithDailyRiding()
        {
            var results = new List<YearlyDetailsDayInfo>();

            for (DateTime d = new DateTime(2012, 1, 1); d <= new DateTime(2016, 12, 31); d = d.AddDays(1))
                results.Add(new YearlyDetailsDayInfo() { Date = d, Sport = SportType.Ride, Distance = 1000 });

            var mock = new Mock<IActivityDtoRepository>();

            mock.Setup(x => x.GetYearToDateInfo(USER_ID, null))
                .Returns(results);

            return mock;
        }


        private Mock<IActivityDtoRepository> DataWithGapInDays()
        {
            var results = new List<YearlyDetailsDayInfo>();

                results.Add(new YearlyDetailsDayInfo() { Date = new DateTime(2016,2,1), Sport = SportType.Ride, Distance = 1000 });
            results.Add(new YearlyDetailsDayInfo() { Date = new DateTime(2016, 2, 10), Sport = SportType.Ride, Distance = 1000 });

            var mock = new Mock<IActivityDtoRepository>();

            mock.Setup(x => x.GetYearToDateInfo(USER_ID, null))
                .Returns(results);

            return mock;
        }
    }
}
