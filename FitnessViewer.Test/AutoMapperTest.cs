using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FitnessViewer.Infrastructure.Helpers;
using FitnessViewer.Infrastructure.Models;

namespace FitnessViewer.Test
{
    [TestClass]
    public class AutoMapperTest
    {

        [TestInitialize]
        public void Setup()
        {

            AutoMapperConfiguration.Initialise();
        }

        /// <summary>
        /// Check ActivityPeakDetailCalculator is correctly mapped to ActivityPeakDetail.  Test is to verify that EndIndex is
        /// copied correctly as it's calculated from StartIndex + Duration
        /// </summary>
        [TestMethod]
        public void ActivityPeakDetails()
        {
            ActivityPeakDetailCalculator calc = new ActivityPeakDetailCalculator(10001, Infrastructure.enums.PeakStreamType.Power, 10);
            calc.StartIndex = 1000;
            calc.StreamType = Infrastructure.enums.PeakStreamType.Power;
            calc.Value = 100;

            Assert.AreEqual(1009, calc.EndIndex);

            ActivityPeakDetail mapped = AutoMapper.Mapper.Map<ActivityPeakDetail>(calc);

            Assert.AreEqual(calc.ActivityId, mapped.ActivityId);
            Assert.AreEqual(calc.Duration, mapped.Duration);
            Assert.AreEqual(calc.StartIndex, mapped.StartIndex);
            Assert.AreEqual(calc.EndIndex, mapped.EndIndex);
            Assert.AreEqual(calc.StreamType, mapped.StreamType);
            Assert.AreEqual(calc.Value, mapped.Value);
        }
    }
}
