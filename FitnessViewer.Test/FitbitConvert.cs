using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Fitbit.Models;
using FitnessViewer.Infrastructure.Helpers.Conversions;

namespace FitnessViewer.Test
{
    [TestClass]
    public class FitbitConvert
    {
        [TestMethod]
        public void Fitbit_TimeToBed()
        {
            Assert.AreEqual(FitbitConversion.InvalidValue, FitbitConversion.ConvertFitbitValue(TimeSeriesResourceType.TimeEnteredBed, ""), "No Value");
            Assert.AreEqual(FitbitConversion.InvalidValue, FitbitConversion.ConvertFitbitValue(TimeSeriesResourceType.TimeEnteredBed, "0115"), "Invalid Format");
            Assert.AreEqual(FitbitConversion.InvalidValue, FitbitConversion.ConvertFitbitValue(TimeSeriesResourceType.TimeEnteredBed, "24:00"), "Invalid Hours");
            Assert.AreEqual(FitbitConversion.InvalidValue, FitbitConversion.ConvertFitbitValue(TimeSeriesResourceType.TimeEnteredBed, "01:60"), "Invalid Minutes");
            Assert.AreEqual(FitbitConversion.InvalidValue, FitbitConversion.ConvertFitbitValue(TimeSeriesResourceType.TimeEnteredBed, "01:-1"), "Negative Minutes");


            Assert.AreEqual(0M, FitbitConversion.ConvertFitbitValue(TimeSeriesResourceType.TimeEnteredBed, "00:00"));
            Assert.AreEqual(1M, FitbitConversion.ConvertFitbitValue(TimeSeriesResourceType.TimeEnteredBed, "00:01"));
            Assert.AreEqual(60M, FitbitConversion.ConvertFitbitValue(TimeSeriesResourceType.TimeEnteredBed, "01:00"));
            Assert.AreEqual(60M, FitbitConversion.ConvertFitbitValue(TimeSeriesResourceType.TimeEnteredBed, "01:00"));
            Assert.AreEqual(1439M, FitbitConversion.ConvertFitbitValue(TimeSeriesResourceType.TimeEnteredBed, "23:59"));
            Assert.AreEqual(720M, FitbitConversion.ConvertFitbitValue(TimeSeriesResourceType.TimeEnteredBed, "12:00"));
        }
    }
}
