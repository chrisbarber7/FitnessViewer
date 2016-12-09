using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FitnessViewer.Infrastructure.Helpers;
using System.Linq;
using Moq;
using FitnessViewer.Infrastructure.Intefaces;
using System.Collections.Generic;

namespace FitnessViewer.Test
{
    [TestClass]
    public class TrainingLoadTest
    {
        private DateTime _startContantValues;
        private DateTime _endConstantValues;

        private DateTime _startActualValues;
        private DateTime _endActualValues;
        private const string USER_ID = "uid";

        [TestInitialize]
        public void Setup()
        {
            _startContantValues = DateTime.Now.Date;
            _endConstantValues = DateTime.Now.AddDays(60).Date;

            _startActualValues = new DateTime(2016, 9, 3);
            _endActualValues = new DateTime(2016, 12, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Invalid Start / End Dates")]
        public void CreateAndInitialiseInvalidDates()
        {
            TrainingLoad pmc = new TrainingLoad();
            pmc.Setup(USER_ID, DateTime.Now, DateTime.Now.AddDays(-1));
        }

        [TestMethod]

        public void CreateAndInitialiseValidDates()
        {
            TrainingLoad pmc = new TrainingLoad();
            pmc.Setup(USER_ID, _startContantValues, _endConstantValues);
      
            // test start and end dates exist. 
            Assert.IsNotNull(pmc.DayValues.Where(d => d.Date == _startContantValues).FirstOrDefault());
            Assert.IsNotNull(pmc.DayValues.Where(d => d.Date == _endConstantValues).FirstOrDefault());

            // check each date exists in collection
            for (DateTime date = _startContantValues; date <= _endConstantValues; date = date.AddDays(1))
                Assert.IsNotNull(pmc.DayValues.Where(d => d.Date == date).FirstOrDefault());
        }

        [TestMethod]
        public void TSSValueLoadTest()
        {

            Mock<IActivityDtoRepository> mock = Constant100TSSDaily();

            TrainingLoad pmc = new TrainingLoad(mock.Object);
            pmc.Setup(USER_ID, _startContantValues, _endConstantValues);
            pmc.Calculate("Ride");

            // check TSS values are populated correctly.
            foreach (TrainingLoadDay day in pmc.DayValues)
                Assert.AreEqual(100, day.TSS);
        }

        [TestMethod]
        public void ConstantTSSTest()
        {

            Mock<IActivityDtoRepository> mock = Constant100TSSDaily();

            TrainingLoad pmc = new TrainingLoad(mock.Object);
            pmc.Setup(USER_ID, _startContantValues, _endConstantValues);
            pmc.Calculate("Ride");

            Assert.AreEqual(13.31M, pmc.DayValues.Where(d => d.Date == _startContantValues).Select(d => d.ShortTermLoad).FirstOrDefault());
            Assert.AreEqual(24.85M, pmc.DayValues.Where(d => d.Date == _startContantValues.AddDays(1)).Select(d => d.ShortTermLoad).FirstOrDefault());
            Assert.AreEqual(34.86M, pmc.DayValues.Where(d => d.Date == _startContantValues.AddDays(2)).Select(d => d.ShortTermLoad).FirstOrDefault());
            Assert.AreEqual(43.53M, pmc.DayValues.Where(d => d.Date == _startContantValues.AddDays(3)).Select(d => d.ShortTermLoad).FirstOrDefault());
            Assert.AreEqual(51.05M, pmc.DayValues.Where(d => d.Date == _startContantValues.AddDays(4)).Select(d => d.ShortTermLoad).FirstOrDefault());
            Assert.AreEqual(57.56M, pmc.DayValues.Where(d => d.Date == _startContantValues.AddDays(5)).Select(d => d.ShortTermLoad).FirstOrDefault());
            Assert.AreEqual(63.21M, pmc.DayValues.Where(d => d.Date == _startContantValues.AddDays(6)).Select(d => d.ShortTermLoad).FirstOrDefault());
            Assert.AreEqual(68.11M, pmc.DayValues.Where(d => d.Date == _startContantValues.AddDays(7)).Select(d => d.ShortTermLoad).FirstOrDefault());
            Assert.AreEqual(72.35M, pmc.DayValues.Where(d => d.Date == _startContantValues.AddDays(8)).Select(d => d.ShortTermLoad).FirstOrDefault());
            Assert.AreEqual(76.03M, pmc.DayValues.Where(d => d.Date == _startContantValues.AddDays(9)).Select(d => d.ShortTermLoad).FirstOrDefault());
            Assert.AreEqual(79.23M, pmc.DayValues.Where(d => d.Date == _startContantValues.AddDays(10)).Select(d => d.ShortTermLoad).FirstOrDefault());
            Assert.AreEqual(81.99M, pmc.DayValues.Where(d => d.Date == _startContantValues.AddDays(11)).Select(d => d.ShortTermLoad).FirstOrDefault());
            Assert.AreEqual(84.39M, pmc.DayValues.Where(d => d.Date == _startContantValues.AddDays(12)).Select(d => d.ShortTermLoad).FirstOrDefault());
            Assert.AreEqual(86.47M, pmc.DayValues.Where(d => d.Date == _startContantValues.AddDays(13)).Select(d => d.ShortTermLoad).FirstOrDefault());
            Assert.AreEqual(88.27M, pmc.DayValues.Where(d => d.Date == _startContantValues.AddDays(14)).Select(d => d.ShortTermLoad).FirstOrDefault());
            Assert.AreEqual(89.83M, pmc.DayValues.Where(d => d.Date == _startContantValues.AddDays(15)).Select(d => d.ShortTermLoad).FirstOrDefault());
            Assert.AreEqual(91.18M, pmc.DayValues.Where(d => d.Date == _startContantValues.AddDays(16)).Select(d => d.ShortTermLoad).FirstOrDefault());
            Assert.AreEqual(92.36M, pmc.DayValues.Where(d => d.Date == _startContantValues.AddDays(17)).Select(d => d.ShortTermLoad).FirstOrDefault());
            Assert.AreEqual(93.37M, pmc.DayValues.Where(d => d.Date == _startContantValues.AddDays(18)).Select(d => d.ShortTermLoad).FirstOrDefault());
            Assert.AreEqual(94.26M, pmc.DayValues.Where(d => d.Date == _startContantValues.AddDays(19)).Select(d => d.ShortTermLoad).FirstOrDefault());
            Assert.AreEqual(95.02M, pmc.DayValues.Where(d => d.Date == _startContantValues.AddDays(20)).Select(d => d.ShortTermLoad).FirstOrDefault());
            Assert.AreEqual(95.68M, pmc.DayValues.Where(d => d.Date == _startContantValues.AddDays(21)).Select(d => d.ShortTermLoad).FirstOrDefault());
            Assert.AreEqual(96.26M, pmc.DayValues.Where(d => d.Date == _startContantValues.AddDays(22)).Select(d => d.ShortTermLoad).FirstOrDefault());
            Assert.AreEqual(96.76M, pmc.DayValues.Where(d => d.Date == _startContantValues.AddDays(23)).Select(d => d.ShortTermLoad).FirstOrDefault());
            Assert.AreEqual(97.19M, pmc.DayValues.Where(d => d.Date == _startContantValues.AddDays(24)).Select(d => d.ShortTermLoad).FirstOrDefault());
            Assert.AreEqual(97.56M, pmc.DayValues.Where(d => d.Date == _startContantValues.AddDays(25)).Select(d => d.ShortTermLoad).FirstOrDefault());
            Assert.AreEqual(97.89M, pmc.DayValues.Where(d => d.Date == _startContantValues.AddDays(26)).Select(d => d.ShortTermLoad).FirstOrDefault());
            Assert.AreEqual(98.17M, pmc.DayValues.Where(d => d.Date == _startContantValues.AddDays(27)).Select(d => d.ShortTermLoad).FirstOrDefault());
            Assert.AreEqual(98.41M, pmc.DayValues.Where(d => d.Date == _startContantValues.AddDays(28)).Select(d => d.ShortTermLoad).FirstOrDefault());
            Assert.AreEqual(98.62M, pmc.DayValues.Where(d => d.Date == _startContantValues.AddDays(29)).Select(d => d.ShortTermLoad).FirstOrDefault());
            Assert.AreEqual(98.81M, pmc.DayValues.Where(d => d.Date == _startContantValues.AddDays(30)).Select(d => d.ShortTermLoad).FirstOrDefault());
            Assert.AreEqual(98.97M, pmc.DayValues.Where(d => d.Date == _startContantValues.AddDays(31)).Select(d => d.ShortTermLoad).FirstOrDefault());
            Assert.AreEqual(99.10M, pmc.DayValues.Where(d => d.Date == _startContantValues.AddDays(32)).Select(d => d.ShortTermLoad).FirstOrDefault());
            Assert.AreEqual(99.22M, pmc.DayValues.Where(d => d.Date == _startContantValues.AddDays(33)).Select(d => d.ShortTermLoad).FirstOrDefault());
            Assert.AreEqual(99.33M, pmc.DayValues.Where(d => d.Date == _startContantValues.AddDays(34)).Select(d => d.ShortTermLoad).FirstOrDefault());
            Assert.AreEqual(99.42M, pmc.DayValues.Where(d => d.Date == _startContantValues.AddDays(35)).Select(d => d.ShortTermLoad).FirstOrDefault());
            Assert.AreEqual(99.49M, pmc.DayValues.Where(d => d.Date == _startContantValues.AddDays(36)).Select(d => d.ShortTermLoad).FirstOrDefault());
            Assert.AreEqual(99.56M, pmc.DayValues.Where(d => d.Date == _startContantValues.AddDays(37)).Select(d => d.ShortTermLoad).FirstOrDefault());
            Assert.AreEqual(99.62M, pmc.DayValues.Where(d => d.Date == _startContantValues.AddDays(38)).Select(d => d.ShortTermLoad).FirstOrDefault());
            Assert.AreEqual(99.67M, pmc.DayValues.Where(d => d.Date == _startContantValues.AddDays(39)).Select(d => d.ShortTermLoad).FirstOrDefault());
            Assert.AreEqual(99.71M, pmc.DayValues.Where(d => d.Date == _startContantValues.AddDays(40)).Select(d => d.ShortTermLoad).FirstOrDefault());
            Assert.AreEqual(99.75M, pmc.DayValues.Where(d => d.Date == _startContantValues.AddDays(41)).Select(d => d.ShortTermLoad).FirstOrDefault());
            Assert.AreEqual(99.79M, pmc.DayValues.Where(d => d.Date == _startContantValues.AddDays(42)).Select(d => d.ShortTermLoad).FirstOrDefault());
            Assert.AreEqual(99.81M, pmc.DayValues.Where(d => d.Date == _startContantValues.AddDays(43)).Select(d => d.ShortTermLoad).FirstOrDefault());
            Assert.AreEqual(99.84M, pmc.DayValues.Where(d => d.Date == _startContantValues.AddDays(44)).Select(d => d.ShortTermLoad).FirstOrDefault());
            Assert.AreEqual(99.86M, pmc.DayValues.Where(d => d.Date == _startContantValues.AddDays(45)).Select(d => d.ShortTermLoad).FirstOrDefault());
            Assert.AreEqual(99.88M, pmc.DayValues.Where(d => d.Date == _startContantValues.AddDays(46)).Select(d => d.ShortTermLoad).FirstOrDefault());
            Assert.AreEqual(99.89M, pmc.DayValues.Where(d => d.Date == _startContantValues.AddDays(47)).Select(d => d.ShortTermLoad).FirstOrDefault());
            Assert.AreEqual(99.91M, pmc.DayValues.Where(d => d.Date == _startContantValues.AddDays(48)).Select(d => d.ShortTermLoad).FirstOrDefault());
            Assert.AreEqual(99.92M, pmc.DayValues.Where(d => d.Date == _startContantValues.AddDays(49)).Select(d => d.ShortTermLoad).FirstOrDefault());
            Assert.AreEqual(99.93M, pmc.DayValues.Where(d => d.Date == _startContantValues.AddDays(50)).Select(d => d.ShortTermLoad).FirstOrDefault());
            Assert.AreEqual(99.94M, pmc.DayValues.Where(d => d.Date == _startContantValues.AddDays(51)).Select(d => d.ShortTermLoad).FirstOrDefault());
            Assert.AreEqual(99.95M, pmc.DayValues.Where(d => d.Date == _startContantValues.AddDays(52)).Select(d => d.ShortTermLoad).FirstOrDefault());
            Assert.AreEqual(99.96M, pmc.DayValues.Where(d => d.Date == _startContantValues.AddDays(53)).Select(d => d.ShortTermLoad).FirstOrDefault());
            Assert.AreEqual(99.96M, pmc.DayValues.Where(d => d.Date == _startContantValues.AddDays(54)).Select(d => d.ShortTermLoad).FirstOrDefault());
            Assert.AreEqual(99.97M, pmc.DayValues.Where(d => d.Date == _startContantValues.AddDays(55)).Select(d => d.ShortTermLoad).FirstOrDefault());
            Assert.AreEqual(99.97M, pmc.DayValues.Where(d => d.Date == _startContantValues.AddDays(56)).Select(d => d.ShortTermLoad).FirstOrDefault());
            Assert.AreEqual(99.97M, pmc.DayValues.Where(d => d.Date == _startContantValues.AddDays(57)).Select(d => d.ShortTermLoad).FirstOrDefault());
            Assert.AreEqual(99.98M, pmc.DayValues.Where(d => d.Date == _startContantValues.AddDays(58)).Select(d => d.ShortTermLoad).FirstOrDefault());
            Assert.AreEqual(99.98M, pmc.DayValues.Where(d => d.Date == _startContantValues.AddDays(59)).Select(d => d.ShortTermLoad).FirstOrDefault());
            Assert.AreEqual(99.98M, pmc.DayValues.Where(d => d.Date == _startContantValues.AddDays(60)).Select(d => d.ShortTermLoad).FirstOrDefault());



            Assert.AreEqual(2.35M,  pmc.DayValues.Where(d => d.Date == _startContantValues.AddDays(0)).Select(d => d.LongTermLoad).FirstOrDefault());
            Assert.AreEqual(4.65M,  pmc.DayValues.Where(d => d.Date == _startContantValues.AddDays(1)).Select(d => d.LongTermLoad).FirstOrDefault());
            Assert.AreEqual(6.89M,  pmc.DayValues.Where(d => d.Date == _startContantValues.AddDays(2)).Select(d => d.LongTermLoad).FirstOrDefault());
            Assert.AreEqual(9.08M,  pmc.DayValues.Where(d => d.Date == _startContantValues.AddDays(3)).Select(d => d.LongTermLoad).FirstOrDefault());
            Assert.AreEqual(11.22M,  pmc.DayValues.Where(d => d.Date == _startContantValues.AddDays(4)).Select(d => d.LongTermLoad).FirstOrDefault());
            Assert.AreEqual(13.31M , pmc.DayValues.Where(d => d.Date == _startContantValues.AddDays(5)).Select(d => d.LongTermLoad).FirstOrDefault());
            Assert.AreEqual(15.35M , pmc.DayValues.Where(d => d.Date == _startContantValues.AddDays(6)).Select(d => d.LongTermLoad).FirstOrDefault());
            Assert.AreEqual(17.34M , pmc.DayValues.Where(d => d.Date == _startContantValues.AddDays(7)).Select(d => d.LongTermLoad).FirstOrDefault());
            Assert.AreEqual(19.29M , pmc.DayValues.Where(d => d.Date == _startContantValues.AddDays(8)).Select(d => d.LongTermLoad).FirstOrDefault());
            Assert.AreEqual(21.19M , pmc.DayValues.Where(d => d.Date == _startContantValues.AddDays(9)).Select(d => d.LongTermLoad).FirstOrDefault());
            Assert.AreEqual(23.04M, pmc.DayValues.Where(d => d.Date == _startContantValues.AddDays(10)).Select(d => d.LongTermLoad).FirstOrDefault());
            Assert.AreEqual(24.85M, pmc.DayValues.Where(d => d.Date == _startContantValues.AddDays(11)).Select(d => d.LongTermLoad).FirstOrDefault());
            Assert.AreEqual(26.62M, pmc.DayValues.Where(d => d.Date == _startContantValues.AddDays(12)).Select(d => d.LongTermLoad).FirstOrDefault());
            Assert.AreEqual(28.35M, pmc.DayValues.Where(d => d.Date == _startContantValues.AddDays(13)).Select(d => d.LongTermLoad).FirstOrDefault());
            Assert.AreEqual(30.03M, pmc.DayValues.Where(d => d.Date == _startContantValues.AddDays(14)).Select(d => d.LongTermLoad).FirstOrDefault());
            Assert.AreEqual(31.68M, pmc.DayValues.Where(d => d.Date == _startContantValues.AddDays(15)).Select(d => d.LongTermLoad).FirstOrDefault());
            Assert.AreEqual(33.29M, pmc.DayValues.Where(d => d.Date == _startContantValues.AddDays(16)).Select(d => d.LongTermLoad).FirstOrDefault());
            Assert.AreEqual(34.86M, pmc.DayValues.Where(d => d.Date == _startContantValues.AddDays(17)).Select(d => d.LongTermLoad).FirstOrDefault());
            Assert.AreEqual(36.39M, pmc.DayValues.Where(d => d.Date == _startContantValues.AddDays(18)).Select(d => d.LongTermLoad).FirstOrDefault());
            Assert.AreEqual(37.89M, pmc.DayValues.Where(d => d.Date == _startContantValues.AddDays(19)).Select(d => d.LongTermLoad).FirstOrDefault());
            Assert.AreEqual(39.35M, pmc.DayValues.Where(d => d.Date == _startContantValues.AddDays(20)).Select(d => d.LongTermLoad).FirstOrDefault());
            Assert.AreEqual(40.77M, pmc.DayValues.Where(d => d.Date == _startContantValues.AddDays(21)).Select(d => d.LongTermLoad).FirstOrDefault());
            Assert.AreEqual(42.17M, pmc.DayValues.Where(d => d.Date == _startContantValues.AddDays(22)).Select(d => d.LongTermLoad).FirstOrDefault());
            Assert.AreEqual(43.53M, pmc.DayValues.Where(d => d.Date == _startContantValues.AddDays(23)).Select(d => d.LongTermLoad).FirstOrDefault());
            Assert.AreEqual(44.86M, pmc.DayValues.Where(d => d.Date == _startContantValues.AddDays(24)).Select(d => d.LongTermLoad).FirstOrDefault());
            Assert.AreEqual(46.15M, pmc.DayValues.Where(d => d.Date == _startContantValues.AddDays(25)).Select(d => d.LongTermLoad).FirstOrDefault());
            Assert.AreEqual(47.42M, pmc.DayValues.Where(d => d.Date == _startContantValues.AddDays(26)).Select(d => d.LongTermLoad).FirstOrDefault());
            Assert.AreEqual(48.66M, pmc.DayValues.Where(d => d.Date == _startContantValues.AddDays(27)).Select(d => d.LongTermLoad).FirstOrDefault());
            Assert.AreEqual(49.87M, pmc.DayValues.Where(d => d.Date == _startContantValues.AddDays(28)).Select(d => d.LongTermLoad).FirstOrDefault());
            Assert.AreEqual(51.05M, pmc.DayValues.Where(d => d.Date == _startContantValues.AddDays(29)).Select(d => d.LongTermLoad).FirstOrDefault());
            Assert.AreEqual(52.20M, pmc.DayValues.Where(d => d.Date == _startContantValues.AddDays(30)).Select(d => d.LongTermLoad).FirstOrDefault());
            Assert.AreEqual(53.32M, pmc.DayValues.Where(d => d.Date == _startContantValues.AddDays(31)).Select(d => d.LongTermLoad).FirstOrDefault());
            Assert.AreEqual(54.42M, pmc.DayValues.Where(d => d.Date == _startContantValues.AddDays(32)).Select(d => d.LongTermLoad).FirstOrDefault());
            Assert.AreEqual(55.49M, pmc.DayValues.Where(d => d.Date == _startContantValues.AddDays(33)).Select(d => d.LongTermLoad).FirstOrDefault());
            Assert.AreEqual(56.54M, pmc.DayValues.Where(d => d.Date == _startContantValues.AddDays(34)).Select(d => d.LongTermLoad).FirstOrDefault());
            Assert.AreEqual(57.56M, pmc.DayValues.Where(d => d.Date == _startContantValues.AddDays(35)).Select(d => d.LongTermLoad).FirstOrDefault());
            Assert.AreEqual(58.56M, pmc.DayValues.Where(d => d.Date == _startContantValues.AddDays(36)).Select(d => d.LongTermLoad).FirstOrDefault());
            Assert.AreEqual(59.54M, pmc.DayValues.Where(d => d.Date == _startContantValues.AddDays(37)).Select(d => d.LongTermLoad).FirstOrDefault());
            Assert.AreEqual(60.49M, pmc.DayValues.Where(d => d.Date == _startContantValues.AddDays(38)).Select(d => d.LongTermLoad).FirstOrDefault());
            Assert.AreEqual(61.42M, pmc.DayValues.Where(d => d.Date == _startContantValues.AddDays(39)).Select(d => d.LongTermLoad).FirstOrDefault());
            Assert.AreEqual(62.33M, pmc.DayValues.Where(d => d.Date == _startContantValues.AddDays(40)).Select(d => d.LongTermLoad).FirstOrDefault());
            Assert.AreEqual(63.21M, pmc.DayValues.Where(d => d.Date == _startContantValues.AddDays(41)).Select(d => d.LongTermLoad).FirstOrDefault());
            Assert.AreEqual(64.08M, pmc.DayValues.Where(d => d.Date == _startContantValues.AddDays(42)).Select(d => d.LongTermLoad).FirstOrDefault());
            Assert.AreEqual(64.92M, pmc.DayValues.Where(d => d.Date == _startContantValues.AddDays(43)).Select(d => d.LongTermLoad).FirstOrDefault());
            Assert.AreEqual(65.75M, pmc.DayValues.Where(d => d.Date == _startContantValues.AddDays(44)).Select(d => d.LongTermLoad).FirstOrDefault());
            Assert.AreEqual(66.55M, pmc.DayValues.Where(d => d.Date == _startContantValues.AddDays(45)).Select(d => d.LongTermLoad).FirstOrDefault());
            Assert.AreEqual(67.34M, pmc.DayValues.Where(d => d.Date == _startContantValues.AddDays(46)).Select(d => d.LongTermLoad).FirstOrDefault());
            Assert.AreEqual(68.11M, pmc.DayValues.Where(d => d.Date == _startContantValues.AddDays(47)).Select(d => d.LongTermLoad).FirstOrDefault());
            Assert.AreEqual(68.86M, pmc.DayValues.Where(d => d.Date == _startContantValues.AddDays(48)).Select(d => d.LongTermLoad).FirstOrDefault());
            Assert.AreEqual(69.59M, pmc.DayValues.Where(d => d.Date == _startContantValues.AddDays(49)).Select(d => d.LongTermLoad).FirstOrDefault());
            Assert.AreEqual(70.31M, pmc.DayValues.Where(d => d.Date == _startContantValues.AddDays(50)).Select(d => d.LongTermLoad).FirstOrDefault());
            Assert.AreEqual(71.01M, pmc.DayValues.Where(d => d.Date == _startContantValues.AddDays(51)).Select(d => d.LongTermLoad).FirstOrDefault());
            Assert.AreEqual(71.69M, pmc.DayValues.Where(d => d.Date == _startContantValues.AddDays(52)).Select(d => d.LongTermLoad).FirstOrDefault());
            Assert.AreEqual(72.35M, pmc.DayValues.Where(d => d.Date == _startContantValues.AddDays(53)).Select(d => d.LongTermLoad).FirstOrDefault());
            Assert.AreEqual(73.01M, pmc.DayValues.Where(d => d.Date == _startContantValues.AddDays(54)).Select(d => d.LongTermLoad).FirstOrDefault());
            Assert.AreEqual(73.64M, pmc.DayValues.Where(d => d.Date == _startContantValues.AddDays(55)).Select(d => d.LongTermLoad).FirstOrDefault());
            Assert.AreEqual(74.26M, pmc.DayValues.Where(d => d.Date == _startContantValues.AddDays(56)).Select(d => d.LongTermLoad).FirstOrDefault());
            Assert.AreEqual(74.87M, pmc.DayValues.Where(d => d.Date == _startContantValues.AddDays(57)).Select(d => d.LongTermLoad).FirstOrDefault());
            Assert.AreEqual(75.46M, pmc.DayValues.Where(d => d.Date == _startContantValues.AddDays(58)).Select(d => d.LongTermLoad).FirstOrDefault());
            Assert.AreEqual(76.03M, pmc.DayValues.Where(d => d.Date == _startContantValues.AddDays(59)).Select(d => d.LongTermLoad).FirstOrDefault());
            Assert.AreEqual(76.60M, pmc.DayValues.Where(d => d.Date == _startContantValues.AddDays(60)).Select(d => d.LongTermLoad).FirstOrDefault());



        }


        [TestMethod]
        public void ActualTSSTest()
        {

            Mock<IActivityDtoRepository> mock = ActualTSS();

            TrainingLoad pmc = new TrainingLoad(mock.Object);

            pmc.Setup(USER_ID, _startActualValues, _endActualValues);
            pmc.ShortTermSeed = 47.2M;
            pmc.LongTermSeed = 46.7M;
            pmc.ShortTermDays = 6.5;

            pmc.Calculate("Ride");

            Assert.AreEqual(40.47M, pmc.DayValues.Where(d => d.Date == new DateTime(2016, 9, 3)).Select(d => d.ShortTermLoad).FirstOrDefault());
            Assert.AreEqual(45.60M, pmc.DayValues.Where(d => d.Date == new DateTime(2016, 9, 3)).Select(d => d.LongTermLoad).FirstOrDefault());
            Assert.AreEqual(58.10M, pmc.DayValues.Where(d => d.Date == new DateTime(2016, 9, 4)).Select(d => d.ShortTermLoad).FirstOrDefault());
            Assert.AreEqual(48.39M, pmc.DayValues.Where(d => d.Date == new DateTime(2016, 9, 4)).Select(d => d.LongTermLoad).FirstOrDefault());
            Assert.AreEqual(49.81M, pmc.DayValues.Where(d => d.Date == new DateTime(2016, 9, 5)).Select(d => d.ShortTermLoad).FirstOrDefault());
            Assert.AreEqual(47.25M, pmc.DayValues.Where(d => d.Date == new DateTime(2016, 9, 5)).Select(d => d.LongTermLoad).FirstOrDefault());
            Assert.AreEqual(48.56M, pmc.DayValues.Where(d => d.Date == new DateTime(2016, 9, 6)).Select(d => d.ShortTermLoad).FirstOrDefault());
            Assert.AreEqual(47.10M, pmc.DayValues.Where(d => d.Date == new DateTime(2016, 9, 6)).Select(d => d.LongTermLoad).FirstOrDefault());
            Assert.AreEqual(46.80M, pmc.DayValues.Where(d => d.Date == new DateTime(2016, 9, 7)).Select(d => d.ShortTermLoad).FirstOrDefault());
            Assert.AreEqual(46.85M, pmc.DayValues.Where(d => d.Date == new DateTime(2016, 9, 7)).Select(d => d.LongTermLoad).FirstOrDefault());
            Assert.AreEqual(40.12M, pmc.DayValues.Where(d => d.Date == new DateTime(2016, 9, 8)).Select(d => d.ShortTermLoad).FirstOrDefault());
            Assert.AreEqual(45.74M, pmc.DayValues.Where(d => d.Date == new DateTime(2016, 9, 8)).Select(d => d.LongTermLoad).FirstOrDefault());
            Assert.AreEqual(40.86M, pmc.DayValues.Where(d => d.Date == new DateTime(2016, 9, 9)).Select(d => d.ShortTermLoad).FirstOrDefault());
            Assert.AreEqual(45.73M, pmc.DayValues.Where(d => d.Date == new DateTime(2016, 9, 9)).Select(d => d.LongTermLoad).FirstOrDefault());
            Assert.AreEqual(35.03M, pmc.DayValues.Where(d => d.Date == new DateTime(2016, 9, 10)).Select(d => d.ShortTermLoad).FirstOrDefault());
            Assert.AreEqual(44.66M, pmc.DayValues.Where(d => d.Date == new DateTime(2016, 9, 10)).Select(d => d.LongTermLoad).FirstOrDefault());
            Assert.AreEqual(30.04M, pmc.DayValues.Where(d => d.Date == new DateTime(2016, 9, 11)).Select(d => d.ShortTermLoad).FirstOrDefault());
            Assert.AreEqual(43.61M, pmc.DayValues.Where(d => d.Date == new DateTime(2016, 9, 11)).Select(d => d.LongTermLoad).FirstOrDefault());
            Assert.AreEqual(25.75M, pmc.DayValues.Where(d => d.Date == new DateTime(2016, 9, 12)).Select(d => d.ShortTermLoad).FirstOrDefault());
            Assert.AreEqual(42.58M, pmc.DayValues.Where(d => d.Date == new DateTime(2016, 9, 12)).Select(d => d.LongTermLoad).FirstOrDefault());
            Assert.AreEqual(37.41M, pmc.DayValues.Where(d => d.Date == new DateTime(2016, 9, 13)).Select(d => d.ShortTermLoad).FirstOrDefault());
            Assert.AreEqual(44.11M, pmc.DayValues.Where(d => d.Date == new DateTime(2016, 9, 13)).Select(d => d.LongTermLoad).FirstOrDefault());
            Assert.AreEqual(39.71M, pmc.DayValues.Where(d => d.Date == new DateTime(2016, 9, 14)).Select(d => d.ShortTermLoad).FirstOrDefault());
            Assert.AreEqual(44.33M, pmc.DayValues.Where(d => d.Date == new DateTime(2016, 9, 14)).Select(d => d.LongTermLoad).FirstOrDefault());
            Assert.AreEqual(43.21M, pmc.DayValues.Where(d => d.Date == new DateTime(2016, 9, 15)).Select(d => d.ShortTermLoad).FirstOrDefault());
            Assert.AreEqual(44.80M, pmc.DayValues.Where(d => d.Date == new DateTime(2016, 9, 15)).Select(d => d.LongTermLoad).FirstOrDefault());
            Assert.AreEqual(41.37M, pmc.DayValues.Where(d => d.Date == new DateTime(2016, 9, 16)).Select(d => d.ShortTermLoad).FirstOrDefault());
            Assert.AreEqual(44.46M, pmc.DayValues.Where(d => d.Date == new DateTime(2016, 9, 16)).Select(d => d.LongTermLoad).FirstOrDefault());
            Assert.AreEqual(35.47M, pmc.DayValues.Where(d => d.Date == new DateTime(2016, 9, 17)).Select(d => d.ShortTermLoad).FirstOrDefault());
            Assert.AreEqual(43.41M, pmc.DayValues.Where(d => d.Date == new DateTime(2016, 9, 17)).Select(d => d.LongTermLoad).FirstOrDefault());
            Assert.AreEqual(40.55M, pmc.DayValues.Where(d => d.Date == new DateTime(2016, 9, 18)).Select(d => d.ShortTermLoad).FirstOrDefault());
            Assert.AreEqual(44.06M, pmc.DayValues.Where(d => d.Date == new DateTime(2016, 9, 18)).Select(d => d.LongTermLoad).FirstOrDefault());
            Assert.AreEqual(43.28M, pmc.DayValues.Where(d => d.Date == new DateTime(2016, 9, 19)).Select(d => d.ShortTermLoad).FirstOrDefault());
            Assert.AreEqual(44.43M, pmc.DayValues.Where(d => d.Date == new DateTime(2016, 9, 19)).Select(d => d.LongTermLoad).FirstOrDefault());
            Assert.AreEqual(51.43M, pmc.DayValues.Where(d => d.Date == new DateTime(2016, 9, 20)).Select(d => d.ShortTermLoad).FirstOrDefault());
            Assert.AreEqual(45.75M, pmc.DayValues.Where(d => d.Date == new DateTime(2016, 9, 20)).Select(d => d.LongTermLoad).FirstOrDefault());
            Assert.AreEqual(46.60M, pmc.DayValues.Where(d => d.Date == new DateTime(2016, 9, 21)).Select(d => d.ShortTermLoad).FirstOrDefault());
            Assert.AreEqual(45.09M, pmc.DayValues.Where(d => d.Date == new DateTime(2016, 9, 21)).Select(d => d.LongTermLoad).FirstOrDefault());
            Assert.AreEqual(47.30M, pmc.DayValues.Where(d => d.Date == new DateTime(2016, 9, 22)).Select(d => d.ShortTermLoad).FirstOrDefault());
            Assert.AreEqual(45.24M, pmc.DayValues.Where(d => d.Date == new DateTime(2016, 9, 22)).Select(d => d.LongTermLoad).FirstOrDefault());
            Assert.AreEqual(45.21M, pmc.DayValues.Where(d => d.Date == new DateTime(2016, 9, 23)).Select(d => d.ShortTermLoad).FirstOrDefault());
            Assert.AreEqual(44.94M, pmc.DayValues.Where(d => d.Date == new DateTime(2016, 9, 23)).Select(d => d.LongTermLoad).FirstOrDefault());
            Assert.AreEqual(38.76M, pmc.DayValues.Where(d => d.Date == new DateTime(2016, 9, 24)).Select(d => d.ShortTermLoad).FirstOrDefault());
            Assert.AreEqual(43.88M, pmc.DayValues.Where(d => d.Date == new DateTime(2016, 9, 24)).Select(d => d.LongTermLoad).FirstOrDefault());
            Assert.AreEqual(33.23M, pmc.DayValues.Where(d => d.Date == new DateTime(2016, 9, 25)).Select(d => d.ShortTermLoad).FirstOrDefault());
            Assert.AreEqual(42.85M, pmc.DayValues.Where(d => d.Date == new DateTime(2016, 9, 25)).Select(d => d.LongTermLoad).FirstOrDefault());
            Assert.AreEqual(28.49M, pmc.DayValues.Where(d => d.Date == new DateTime(2016, 9, 26)).Select(d => d.ShortTermLoad).FirstOrDefault());
            Assert.AreEqual(41.84M, pmc.DayValues.Where(d => d.Date == new DateTime(2016, 9, 26)).Select(d => d.LongTermLoad).FirstOrDefault());
            Assert.AreEqual(24.43M, pmc.DayValues.Where(d => d.Date == new DateTime(2016, 9, 27)).Select(d => d.ShortTermLoad).FirstOrDefault());
            Assert.AreEqual(40.86M, pmc.DayValues.Where(d => d.Date == new DateTime(2016, 9, 27)).Select(d => d.LongTermLoad).FirstOrDefault());
            Assert.AreEqual(20.95M, pmc.DayValues.Where(d => d.Date == new DateTime(2016, 9, 28)).Select(d => d.ShortTermLoad).FirstOrDefault());
            Assert.AreEqual(39.90M, pmc.DayValues.Where(d => d.Date == new DateTime(2016, 9, 28)).Select(d => d.LongTermLoad).FirstOrDefault());
            Assert.AreEqual(17.96M, pmc.DayValues.Where(d => d.Date == new DateTime(2016, 9, 29)).Select(d => d.ShortTermLoad).FirstOrDefault());
            Assert.AreEqual(38.96M, pmc.DayValues.Where(d => d.Date == new DateTime(2016, 9, 29)).Select(d => d.LongTermLoad).FirstOrDefault());
            Assert.AreEqual(15.40M, pmc.DayValues.Where(d => d.Date == new DateTime(2016, 9, 30)).Select(d => d.ShortTermLoad).FirstOrDefault());
            Assert.AreEqual(38.04M, pmc.DayValues.Where(d => d.Date == new DateTime(2016, 9, 30)).Select(d => d.LongTermLoad).FirstOrDefault());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Invalid ShortTermSeed")]
        public void SeedATLTest()
        {
            TrainingLoad pmc = new TrainingLoad();
            pmc.ShortTermSeed = -1;
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Invalid LongTermSeed")]
        public void SeedCTLTest()
        {
            TrainingLoad pmc = new TrainingLoad();
            pmc.LongTermSeed = -1;
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Invalid LongTermDays")]
        public void ATLDayConstantTest()
        {
            TrainingLoad pmc = new TrainingLoad();
            pmc.ShortTermDays = -1;
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Invalid LongTermDays")]
        public void CTLDayConstantTest()
        {
            TrainingLoad pmc = new TrainingLoad();
            pmc.LongTermDays = -1;
        }

        private Mock<IActivityDtoRepository> Constant100TSSDaily()
        {
            var results = new List<KeyValuePair<DateTime, decimal>>();

            for (DateTime date = _startContantValues; date <= _endConstantValues; date = date.AddDays(1))
                results.Add(new KeyValuePair<DateTime, decimal>(date, 100M));

            var mock = new Mock<IActivityDtoRepository>();

            mock.Setup(x => x.GetDailyTSS(USER_ID, "Ride", _startContantValues, _endConstantValues))
                             .Returns(results);

            return mock;
        }


        /// <summary>
        /// Test using Actual TSS values from training peaks and comparing ATL/CTL from Training Peaks PMC
        /// </summary>
        /// <returns></returns>
        private Mock<IActivityDtoRepository> ActualTSS()
        {
            var results = new List<KeyValuePair<DateTime, decimal>>();

            results.Add(new KeyValuePair<DateTime, decimal>(new DateTime(2016, 9, 3), 0));
            results.Add(new KeyValuePair<DateTime, decimal>(new DateTime(2016, 9, 4), 164.1M));
            results.Add(new KeyValuePair<DateTime, decimal>(new DateTime(2016, 9, 5), 0));
            results.Add(new KeyValuePair<DateTime, decimal>(new DateTime(2016, 9, 6), 41));
            results.Add(new KeyValuePair<DateTime, decimal>(new DateTime(2016, 9, 7), 36.2M));
            results.Add(new KeyValuePair<DateTime, decimal>(new DateTime(2016, 9, 8), 0));
            results.Add(new KeyValuePair<DateTime, decimal>(new DateTime(2016, 9, 9), 45.3M));
            results.Add(new KeyValuePair<DateTime, decimal>(new DateTime(2016, 9, 10), 0));
            results.Add(new KeyValuePair<DateTime, decimal>(new DateTime(2016, 9, 11), 0));
            results.Add(new KeyValuePair<DateTime, decimal>(new DateTime(2016, 9, 12), 0));
            results.Add(new KeyValuePair<DateTime, decimal>(new DateTime(2016, 9, 13), 107.5M));
            results.Add(new KeyValuePair<DateTime, decimal>(new DateTime(2016, 9, 14), 53.5M));
            results.Add(new KeyValuePair<DateTime, decimal>(new DateTime(2016, 9, 15), 64.3M));
            results.Add(new KeyValuePair<DateTime, decimal>(new DateTime(2016, 9, 16), 30.3M));
            results.Add(new KeyValuePair<DateTime, decimal>(new DateTime(2016, 9, 17), 0));
            results.Add(new KeyValuePair<DateTime, decimal>(new DateTime(2016, 9, 18), 71.1M));
            results.Add(new KeyValuePair<DateTime, decimal>(new DateTime(2016, 9, 19), 59.7M));
            results.Add(new KeyValuePair<DateTime, decimal>(new DateTime(2016, 9, 20), 100.4M));
            results.Add(new KeyValuePair<DateTime, decimal>(new DateTime(2016, 9, 21), 17.6M));
            results.Add(new KeyValuePair<DateTime, decimal>(new DateTime(2016, 9, 22), 51.5M));
            results.Add(new KeyValuePair<DateTime, decimal>(new DateTime(2016, 9, 23), 32.6M));
            results.Add(new KeyValuePair<DateTime, decimal>(new DateTime(2016, 9, 24), 0));
            results.Add(new KeyValuePair<DateTime, decimal>(new DateTime(2016, 9, 25), 0));
            results.Add(new KeyValuePair<DateTime, decimal>(new DateTime(2016, 9, 26), 0));
            results.Add(new KeyValuePair<DateTime, decimal>(new DateTime(2016, 9, 27), 0));
            results.Add(new KeyValuePair<DateTime, decimal>(new DateTime(2016, 9, 28), 0));
            results.Add(new KeyValuePair<DateTime, decimal>(new DateTime(2016, 9, 29), 0));
            results.Add(new KeyValuePair<DateTime, decimal>(new DateTime(2016, 9, 30), 0));


            results.Add(new KeyValuePair<DateTime, decimal>(new DateTime(2016, 10, 1), 0));
            results.Add(new KeyValuePair<DateTime, decimal>(new DateTime(2016, 10, 2), 0));
            results.Add(new KeyValuePair<DateTime, decimal>(new DateTime(2016, 10, 3), 51.6M));
            results.Add(new KeyValuePair<DateTime, decimal>(new DateTime(2016, 10, 4), 0));
            results.Add(new KeyValuePair<DateTime, decimal>(new DateTime(2016, 10, 5), 83.6M));
            results.Add(new KeyValuePair<DateTime, decimal>(new DateTime(2016, 10, 6), 0));
            results.Add(new KeyValuePair<DateTime, decimal>(new DateTime(2016, 10, 7), 0));
            results.Add(new KeyValuePair<DateTime, decimal>(new DateTime(2016, 10, 8), 0));
            results.Add(new KeyValuePair<DateTime, decimal>(new DateTime(2016, 10, 9), 134.6M));
            results.Add(new KeyValuePair<DateTime, decimal>(new DateTime(2016, 10, 10), 62.4M));
            results.Add(new KeyValuePair<DateTime, decimal>(new DateTime(2016, 10, 11), 0));
            results.Add(new KeyValuePair<DateTime, decimal>(new DateTime(2016, 10, 12), 73.8M));
            results.Add(new KeyValuePair<DateTime, decimal>(new DateTime(2016, 10, 13), 97.9M));
            results.Add(new KeyValuePair<DateTime, decimal>(new DateTime(2016, 10, 14), 0));
            results.Add(new KeyValuePair<DateTime, decimal>(new DateTime(2016, 10, 15), 0));
            results.Add(new KeyValuePair<DateTime, decimal>(new DateTime(2016, 10, 16), 0));
            results.Add(new KeyValuePair<DateTime, decimal>(new DateTime(2016, 10, 17), 65.8M));
            results.Add(new KeyValuePair<DateTime, decimal>(new DateTime(2016, 10, 18), 58.1M));
            results.Add(new KeyValuePair<DateTime, decimal>(new DateTime(2016, 10, 19), 36));
            results.Add(new KeyValuePair<DateTime, decimal>(new DateTime(2016, 10, 20), 0));
            results.Add(new KeyValuePair<DateTime, decimal>(new DateTime(2016, 10, 21), 0));
            results.Add(new KeyValuePair<DateTime, decimal>(new DateTime(2016, 10, 22), 0));
            results.Add(new KeyValuePair<DateTime, decimal>(new DateTime(2016, 10, 23), 0));
            results.Add(new KeyValuePair<DateTime, decimal>(new DateTime(2016, 10, 24), 0));
            results.Add(new KeyValuePair<DateTime, decimal>(new DateTime(2016, 10, 25), 150.6M));
            results.Add(new KeyValuePair<DateTime, decimal>(new DateTime(2016, 10, 26), 0));
            results.Add(new KeyValuePair<DateTime, decimal>(new DateTime(2016, 10, 27), 36.7M));
            results.Add(new KeyValuePair<DateTime, decimal>(new DateTime(2016, 10, 28), 0));
            results.Add(new KeyValuePair<DateTime, decimal>(new DateTime(2016, 10, 29), 0));
            results.Add(new KeyValuePair<DateTime, decimal>(new DateTime(2016, 10, 30), 204.8M));
            results.Add(new KeyValuePair<DateTime, decimal>(new DateTime(2016, 10, 31), 0));

            results.Add(new KeyValuePair<DateTime, decimal>(new DateTime(2016, 11, 1), 0));
            results.Add(new KeyValuePair<DateTime, decimal>(new DateTime(2016, 11, 2), 128));
            results.Add(new KeyValuePair<DateTime, decimal>(new DateTime(2016, 11, 3), 60.2M));
            results.Add(new KeyValuePair<DateTime, decimal>(new DateTime(2016, 11, 4), 0));
            results.Add(new KeyValuePair<DateTime, decimal>(new DateTime(2016, 11, 5), 0));
            results.Add(new KeyValuePair<DateTime, decimal>(new DateTime(2016, 11, 6), 0));
            results.Add(new KeyValuePair<DateTime, decimal>(new DateTime(2016, 11, 7), 74.8M));
            results.Add(new KeyValuePair<DateTime, decimal>(new DateTime(2016, 11, 8), 0));
            results.Add(new KeyValuePair<DateTime, decimal>(new DateTime(2016, 11, 9), 62.9M));
            results.Add(new KeyValuePair<DateTime, decimal>(new DateTime(2016, 11, 10), 80.1M));
            results.Add(new KeyValuePair<DateTime, decimal>(new DateTime(2016, 11, 11), 0));
            results.Add(new KeyValuePair<DateTime, decimal>(new DateTime(2016, 11, 12), 0));
            results.Add(new KeyValuePair<DateTime, decimal>(new DateTime(2016, 11, 13), 0));
            results.Add(new KeyValuePair<DateTime, decimal>(new DateTime(2016, 11, 14), 0));
            results.Add(new KeyValuePair<DateTime, decimal>(new DateTime(2016, 11, 15), 117.8M));
            results.Add(new KeyValuePair<DateTime, decimal>(new DateTime(2016, 11, 16), 0));
            results.Add(new KeyValuePair<DateTime, decimal>(new DateTime(2016, 11, 17), 0));
            results.Add(new KeyValuePair<DateTime, decimal>(new DateTime(2016, 11, 18), 0));
            results.Add(new KeyValuePair<DateTime, decimal>(new DateTime(2016, 11, 19), 0));
            results.Add(new KeyValuePair<DateTime, decimal>(new DateTime(2016, 11, 20), 234.8M));
            results.Add(new KeyValuePair<DateTime, decimal>(new DateTime(2016, 11, 21), 0));
            results.Add(new KeyValuePair<DateTime, decimal>(new DateTime(2016, 11, 22), 0));
            results.Add(new KeyValuePair<DateTime, decimal>(new DateTime(2016, 11, 23), 72.2M));
            results.Add(new KeyValuePair<DateTime, decimal>(new DateTime(2016, 11, 24), 54.8M));
            results.Add(new KeyValuePair<DateTime, decimal>(new DateTime(2016, 11, 25), 0));
            results.Add(new KeyValuePair<DateTime, decimal>(new DateTime(2016, 11, 26), 0));
            results.Add(new KeyValuePair<DateTime, decimal>(new DateTime(2016, 11, 27), 0));
            results.Add(new KeyValuePair<DateTime, decimal>(new DateTime(2016, 11, 28), 80));
            results.Add(new KeyValuePair<DateTime, decimal>(new DateTime(2016, 11, 29), 65.5M));
            results.Add(new KeyValuePair<DateTime, decimal>(new DateTime(2016, 11, 30), 59));


            results.Add(new KeyValuePair<DateTime, decimal>(new DateTime(2016, 12, 1), 0));

            var mock = new Mock<IActivityDtoRepository>();

            mock.Setup(x => x.GetDailyTSS(USER_ID, "Ride", _startActualValues, _endActualValues))
                             .Returns(results);

            return mock;
        }

    }
}
