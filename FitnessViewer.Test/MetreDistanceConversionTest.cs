using Microsoft.VisualStudio.TestTools.UnitTesting;
using FitnessViewer.Infrastructure.Helpers.Conversions;

namespace FitnessViewer.Test
{
    [TestClass]
    public class MetreDistanceConversionTest
    {
        [TestMethod]
        public void MetreDistance_MetresToMiles()
        {
            Assert.AreEqual(0.06M, Distance.MetersToMiles(100));
            Assert.AreEqual(0.16M, Distance.MetersToMiles(250));
            Assert.AreEqual(0.31M, Distance.MetersToMiles(500));
            Assert.AreEqual(0.62M, Distance.MetersToMiles(1000));
            Assert.AreEqual(3.11M, Distance.MetersToMiles(5000));
            Assert.AreEqual(6.21M, Distance.MetersToMiles(10000));
        }

        [TestMethod]
        public void MetreDistance_MetersToFeet()
        {
            Assert.AreEqual(32.81M, Distance.MetersToFeet(10));
            Assert.AreEqual(164.04M, Distance.MetersToFeet(50));
            Assert.AreEqual(328.08M, Distance.MetersToFeet(100));
            Assert.AreEqual(2460.63M, Distance.MetersToFeet(750));
            Assert.AreEqual(8202.1M, Distance.MetersToFeet(2500));
        }

        [TestMethod]
        public void MetreDistance_MetersToKM()
        {
            Assert.AreEqual(10, Distance.MetersToKilometers(10000));
            Assert.AreEqual(5, Distance.MetersToKilometers(5000));
        }
    }
}
