using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FitnessViewer.Infrastructure.Helpers;

namespace FitnessViewer.Test
{
    [TestClass]
    public class DisplayLabelTests
    {
        [TestMethod]
        public void DisplayLabel_DurationForDisplayTest()
        {
            Assert.AreEqual("Activity", DisplayLabel.StreamDurationForDisplay(int.MaxValue));
            Assert.AreEqual("5 secs", DisplayLabel.StreamDurationForDisplay(5));
            Assert.AreEqual("30 secs", DisplayLabel.StreamDurationForDisplay(30));
            Assert.AreEqual("1 min", DisplayLabel.StreamDurationForDisplay(60));
            Assert.AreEqual("1 min 1 secs", DisplayLabel.StreamDurationForDisplay(61));
            Assert.AreEqual("2 min", DisplayLabel.StreamDurationForDisplay(120));
            Assert.AreEqual("59 min", DisplayLabel.StreamDurationForDisplay(59*60));
            Assert.AreEqual("59 min 59 secs", DisplayLabel.StreamDurationForDisplay((59 * 60)+59));
            Assert.AreEqual("01:00:00", DisplayLabel.StreamDurationForDisplay(60 * 60));
            Assert.AreEqual("01:00:01", DisplayLabel.StreamDurationForDisplay((60 * 60)+1));
        }
    }
}
