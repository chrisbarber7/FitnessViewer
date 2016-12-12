using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FitnessViewer.Infrastructure.enums;
using FitnessViewer.Infrastructure.Helpers;
using FitnessViewer.Infrastructure.Helpers.Conversions;

namespace FitnessViewer.Test
{
    [TestClass]
    public class SportTypeExtensionMethodTest
    {
        [TestMethod]
        public void SportTypeExtension_EnumToStringValid()
        {
            Assert.AreEqual("Ride", SportType.Ride.ToString());
            Assert.AreEqual("Run", SportType.Run.ToString());
            Assert.AreEqual("Swim", SportType.Swim.ToString());
            Assert.AreEqual("Other", SportType.Other.ToString());
            Assert.AreEqual("All", SportType.All.ToString());
        }

        [TestMethod]
        public void SportTypeExtension_StringToEnumValid()
        {
            Assert.AreEqual(SportType.Ride, EnumConversion.GetEnumFromDescription<SportType>("Ride"));
            Assert.AreEqual(SportType.Run, EnumConversion.GetEnumFromDescription<SportType>("Run"));
            Assert.AreEqual(SportType.Other, EnumConversion.GetEnumFromDescription<SportType>("Other"));
            Assert.AreEqual(SportType.Swim, EnumConversion.GetEnumFromDescription<SportType>("Swim"));
            Assert.AreEqual(SportType.All, EnumConversion.GetEnumFromDescription<SportType>("All"));

        }



        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SportTypeExtension_StringToEnumInvalid()
        {
            Assert.AreEqual(SportType.Ride, EnumConversion.GetEnumFromDescription<SportType>("InvalidSport"));
        }
    }
}
