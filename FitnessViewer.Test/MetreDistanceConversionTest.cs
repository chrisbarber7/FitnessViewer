using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FitnessViewer.Infrastructure.Helpers;

namespace FitnessViewer.Test
{
    [TestClass]
    public class MetreDistanceConversionTest
    {
        [TestMethod]
        public void MetresToMiles()
        {
            Assert.AreEqual(0.06, MetreDistance.ToMiles(100));
            Assert.AreEqual(0.16, MetreDistance.ToMiles(250));
            Assert.AreEqual(0.31, MetreDistance.ToMiles(500));
            Assert.AreEqual(0.62, MetreDistance.ToMiles(1000));
            Assert.AreEqual(3.11, MetreDistance.ToMiles(5000));
            Assert.AreEqual(6.21, MetreDistance.ToMiles(10000));
        }

        [TestMethod]
        public void MetersToFeet()
        {
            Assert.AreEqual(32.81, MetreDistance.ToFeet(10));
            Assert.AreEqual(164.04, MetreDistance.ToFeet(50));
            Assert.AreEqual(328.08, MetreDistance.ToFeet(100));
            Assert.AreEqual(2460.63, MetreDistance.ToFeet(750));
            Assert.AreEqual(8202.1, MetreDistance.ToFeet(2500));
        }

        [TestMethod]
        public void MetersToKM()
        {
            Assert.AreEqual(10, MetreDistance.ToKM(10000));
            Assert.AreEqual(5, MetreDistance.ToKM(5000));
        }
    }
}
