using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FitnessViewer.Infrastructure.Helpers;

namespace FitnessViewer.Test
{
    [TestClass]
    public class PaceCalculatorTest
    {
        public const int MARATHON_DISTANCE = 42195;
        public const int TEN_KM = 10000;
        public const int FIFTEEN_HUNDRED_M = 1500;
        public const int TWO_HUNDRED_M = 200;
        public const int MILE_IN_METRES = 1609;
        public const int NO_DISTANCE = 0;

        [TestMethod]
        public void MarathonTestMinPerMile()
        {
         
            Assert.AreEqual(new TimeSpan(0, 5, 0), PaceCalculator.RunMinuteMiles(MARATHON_DISTANCE, new TimeSpan(2, 11, 6)));
            Assert.AreEqual(new TimeSpan(0, 5, 30), PaceCalculator.RunMinuteMiles(MARATHON_DISTANCE, new TimeSpan(2, 24, 12)));
            Assert.AreEqual(new TimeSpan(0, 6, 0), PaceCalculator.RunMinuteMiles(MARATHON_DISTANCE, new TimeSpan(2, 37, 19)));
            Assert.AreEqual(new TimeSpan(0, 6, 30), PaceCalculator.RunMinuteMiles(MARATHON_DISTANCE, new TimeSpan(2, 50, 25)));
            Assert.AreEqual(new TimeSpan(0, 7, 0), PaceCalculator.RunMinuteMiles(MARATHON_DISTANCE, new TimeSpan(3, 03, 32)));
            Assert.AreEqual(new TimeSpan(0, 7, 30), PaceCalculator.RunMinuteMiles(MARATHON_DISTANCE, new TimeSpan(3, 16, 38)));
            Assert.AreEqual(new TimeSpan(0, 8, 0), PaceCalculator.RunMinuteMiles(MARATHON_DISTANCE, new TimeSpan(3, 29, 45)));
            Assert.AreEqual(new TimeSpan(0, 9, 0), PaceCalculator.RunMinuteMiles(MARATHON_DISTANCE, new TimeSpan(3, 55, 58)));
            Assert.AreEqual(new TimeSpan(0, 10, 0), PaceCalculator.RunMinuteMiles(MARATHON_DISTANCE, new TimeSpan(4, 22, 12)));
        }

        [TestMethod]
        public void TenKmTestMinPerMile()
        {
        
            Assert.AreEqual(new TimeSpan(0, 5, 0), PaceCalculator.RunMinuteMiles(TEN_KM, new TimeSpan(0, 31, 04)));
            Assert.AreEqual(new TimeSpan(0, 5, 30), PaceCalculator.RunMinuteMiles(TEN_KM, new TimeSpan(0, 34, 11)));
            Assert.AreEqual(new TimeSpan(0, 6, 0), PaceCalculator.RunMinuteMiles(TEN_KM, new TimeSpan(0, 37, 17)));
            Assert.AreEqual(new TimeSpan(0, 6, 30), PaceCalculator.RunMinuteMiles(TEN_KM, new TimeSpan(0, 40, 22)));
            Assert.AreEqual(new TimeSpan(0, 7, 0), PaceCalculator.RunMinuteMiles(TEN_KM, new TimeSpan(0, 43, 30)));
            Assert.AreEqual(new TimeSpan(0, 7, 30), PaceCalculator.RunMinuteMiles(TEN_KM, new TimeSpan(0, 46, 36)));
            Assert.AreEqual(new TimeSpan(0, 8, 0), PaceCalculator.RunMinuteMiles(TEN_KM, new TimeSpan(0, 49, 43)));
            Assert.AreEqual(new TimeSpan(0, 9, 0), PaceCalculator.RunMinuteMiles(TEN_KM, new TimeSpan(0, 55, 55)));
            Assert.AreEqual(new TimeSpan(0, 10, 0), PaceCalculator.RunMinuteMiles(TEN_KM, new TimeSpan(1, 02, 08)));
        }


        [TestMethod]
        public void FifteenHundredMetreTestMinPerMile()
        {


            Assert.AreEqual(new TimeSpan(0, 4, 0), PaceCalculator.RunMinuteMiles(FIFTEEN_HUNDRED_M, new TimeSpan(0, 3, 44)));
            Assert.AreEqual(new TimeSpan(0, 4, 30), PaceCalculator.RunMinuteMiles(FIFTEEN_HUNDRED_M, new TimeSpan(0, 4, 12)));
            Assert.AreEqual(new TimeSpan(0, 5, 0), PaceCalculator.RunMinuteMiles(FIFTEEN_HUNDRED_M, new TimeSpan(0, 4, 40)));
            Assert.AreEqual(new TimeSpan(0, 5, 30), PaceCalculator.RunMinuteMiles(FIFTEEN_HUNDRED_M, new TimeSpan(0, 5, 08)));
            Assert.AreEqual(new TimeSpan(0, 6, 0), PaceCalculator.RunMinuteMiles(FIFTEEN_HUNDRED_M, new TimeSpan(0, 5, 36)));
            Assert.AreEqual(new TimeSpan(0, 6, 31), PaceCalculator.RunMinuteMiles(FIFTEEN_HUNDRED_M, new TimeSpan(0, 6, 04)));
            Assert.AreEqual(new TimeSpan(0, 7, 0), PaceCalculator.RunMinuteMiles(FIFTEEN_HUNDRED_M, new TimeSpan(0, 6, 31)));
            Assert.AreEqual(new TimeSpan(0, 7, 30), PaceCalculator.RunMinuteMiles(FIFTEEN_HUNDRED_M, new TimeSpan(0, 6, 59)));
            Assert.AreEqual(new TimeSpan(0, 8, 0), PaceCalculator.RunMinuteMiles(FIFTEEN_HUNDRED_M, new TimeSpan(0, 7, 27)));
            Assert.AreEqual(new TimeSpan(0, 9, 0), PaceCalculator.RunMinuteMiles(FIFTEEN_HUNDRED_M, new TimeSpan(0, 8, 23)));
            Assert.AreEqual(new TimeSpan(0, 10, 0), PaceCalculator.RunMinuteMiles(FIFTEEN_HUNDRED_M, new TimeSpan(0, 09, 19)));
        }

        [TestMethod]
        public void TwoHundredMetreTestMinPerMile()
        {
          

            Assert.AreEqual(new TimeSpan(0, 4, 1), PaceCalculator.RunMinuteMiles(TWO_HUNDRED_M, new TimeSpan(0, 0, 30)));
            Assert.AreEqual(new TimeSpan(0, 4, 42), PaceCalculator.RunMinuteMiles(TWO_HUNDRED_M, new TimeSpan(0, 0, 35)));
            Assert.AreEqual(new TimeSpan(0, 5, 22), PaceCalculator.RunMinuteMiles(TWO_HUNDRED_M, new TimeSpan(0, 0, 40)));
            Assert.AreEqual(new TimeSpan(0, 6, 2), PaceCalculator.RunMinuteMiles(TWO_HUNDRED_M, new TimeSpan(0, 0, 45)));
            Assert.AreEqual(new TimeSpan(0, 6, 42), PaceCalculator.RunMinuteMiles(TWO_HUNDRED_M, new TimeSpan(0, 0, 50)));
            Assert.AreEqual(new TimeSpan(0, 7, 23), PaceCalculator.RunMinuteMiles(TWO_HUNDRED_M, new TimeSpan(0, 0, 55)));
            Assert.AreEqual(new TimeSpan(0, 8, 3), PaceCalculator.RunMinuteMiles(TWO_HUNDRED_M, new TimeSpan(0, 1, 00)));

        }

        [TestMethod]
        public void MileTest()
        {
          

            // test from 210 seconds (3:30min/mile to 600 seconds (10min/mile).  Result shoudl equal input time.
            for (int seconds = 210; seconds <= 600; seconds++)
            {
                Assert.AreEqual(TimeSpan.FromSeconds(seconds), PaceCalculator.RunMinuteMiles(MILE_IN_METRES, TimeSpan.FromSeconds(seconds)));
            }
        }

        [TestMethod]
        public void NoDistance()
        {
            Assert.AreEqual(new TimeSpan(0, 0, 0), PaceCalculator.RunMinuteMiles(NO_DISTANCE, new TimeSpan(0, 1, 00)));
            Assert.AreEqual(new TimeSpan(0, 0, 0), PaceCalculator.RunMinuteMiles(NO_DISTANCE, new TimeSpan(1, 0, 00)));
            Assert.AreEqual(new TimeSpan(0, 0, 0), PaceCalculator.RunMinuteMiles(NO_DISTANCE, new TimeSpan(0, 0, 30)));
        }

        [TestMethod]
        public void NoTime()
        {
            Assert.AreEqual(new TimeSpan(0, 0, 0), PaceCalculator.RunMinuteMiles(TEN_KM, new TimeSpan(0, 0, 0)));
            Assert.AreEqual(new TimeSpan(0, 0, 0), PaceCalculator.RunMinuteMiles(MILE_IN_METRES, new TimeSpan(0, 0, 0)));
            Assert.AreEqual(new TimeSpan(0, 0, 0), PaceCalculator.RunMinuteMiles(TWO_HUNDRED_M, new TimeSpan(0, 0, 0)));
        }
    }
}
