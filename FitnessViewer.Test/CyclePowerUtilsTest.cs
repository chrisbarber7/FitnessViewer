using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using FitnessViewer.Infrastructure.Helpers;
using FitnessViewer.Infrastructure.enums;
using FitnessViewer.Infrastructure.Models;

namespace FitnessViewer.Test
{
    [TestClass]
    public class CyclePowerUtilsTest
    {
        /// <summary>
        /// Test using test data from spreadsheet at 
        /// http://www.timetriallingforum.co.uk/?showtopic=50454
        /// </summary>
        [TestMethod]
        
        public void NormalisedPowerTest()
        {
            List<int> testData = new List<int>();
            for (int x = 1; x <= 4000; x++)
                if (x > 31 && x <= 39)
                    testData.Add(300);
                else
                    testData.Add(200);

            Assert.AreEqual(4000, testData.Count);


            ActivityAnalytics cal = new ActivityAnalytics(testData, 295);
       


            Assert.AreEqual(200.241M, cal.NP());
        }
    }
}