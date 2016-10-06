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
            Assert.AreEqual(0.06M, MetreDistance.ToMiles(100));
            Assert.AreEqual(0.16M, MetreDistance.ToMiles(250));
            Assert.AreEqual(0.31M, MetreDistance.ToMiles(500));
            Assert.AreEqual(0.62M, MetreDistance.ToMiles(1000));
            Assert.AreEqual(3.11M, MetreDistance.ToMiles(5000));
            Assert.AreEqual(6.21M, MetreDistance.ToMiles(10000));
        }

        [TestMethod]
        public void MetersToFeet()
        {
            Assert.AreEqual(32.81M, MetreDistance.ToFeet(10));
            Assert.AreEqual(164.04M, MetreDistance.ToFeet(50));
            Assert.AreEqual(328.08M, MetreDistance.ToFeet(100));
            Assert.AreEqual(2460.63M, MetreDistance.ToFeet(750));
            Assert.AreEqual(8202.1M, MetreDistance.ToFeet(2500));
        }

        [TestMethod]
        public void MetersToKM()
        {
            Assert.AreEqual(10, MetreDistance.ToKM(10000));
            Assert.AreEqual(5, MetreDistance.ToKM(5000));
        }
    }
}
