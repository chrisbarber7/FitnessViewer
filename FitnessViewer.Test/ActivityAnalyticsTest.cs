using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using FitnessViewer.Infrastructure.Helpers;
using FitnessViewer.Infrastructure.enums;
using FitnessViewer.Infrastructure.Models;
using System;
using FitnessViewer.Infrastructure.Helpers.Analytics;

namespace FitnessViewer.Test
{
    [TestClass]
    public class ActivityAnalyticsTest
    {



        /// <summary>
        /// Test using test data from spreadsheet at 
        /// http://www.timetriallingforum.co.uk/?showtopic=50454
        /// </summary>
        [TestMethod]        
        public void NormalisedPowerSpreadsheetTest()
        {
            List<int> testData = new List<int>();
            for (int x = 1; x <= 4000; x++)
                if (x > 31 && x <= 39)
                    testData.Add(300);
                else
                    testData.Add(200);

            Assert.AreEqual(4000, testData.Count);
            BikePower cal = new BikePower(testData, 295);
            Assert.AreEqual(200.241M, cal.NP());
        }


        /// <summary>
        /// One hour at FTP should return NP=FTP, TSS=100 & IF=1
        /// </summary>
        [TestMethod]
        public void OneHourAtFTPTest()
        {
            int ftp = 300;

            List<int> testData = new List<int>();
            for (int x = 1; x <= 60 * 60; x++)
                testData.Add(ftp);

            Assert.AreEqual(60*60, testData.Count);

            BikePower cal = new BikePower(testData,ftp);
            Assert.AreEqual(ftp, cal.NP());
            Assert.AreEqual(100, cal.TSS());
            Assert.AreEqual(1.00M, cal.IntensityFactor());
        }

        /// <summary>
        /// Test against values for activity in TrainingPeaks http://tpks.ws/rhRZ6
        /// Equivalent Strava activity  https://www.strava.com/activities/718678832
        /// </summary>
        [TestMethod]
        public void ActivityTest()
        {
            int ftp = 295;  // ftp at time of activity;
            List<int> testData = FindPeakTest.GetActivityPowerStream();
            
            // results rounded to match values shown on Training Peaks.
            BikePower cal = new BikePower(testData, ftp);
            Assert.AreEqual(227, Math.Round(cal.NP(), 0));
            Assert.AreEqual(52.8M, Math.Round(cal.TSS(), 1));
            Assert.AreEqual(0.77M, Math.Round(cal.IntensityFactor(), 2));
        }
    }
}