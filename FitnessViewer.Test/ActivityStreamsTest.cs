using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using FitnessViewer.Infrastructure.Helpers;
using FitnessViewer.Infrastructure.enums;
using FitnessViewer.Infrastructure.Models;
using System;
using FitnessViewer.Infrastructure.Models.Collections;

namespace FitnessViewer.Test
{
    [TestClass]
    public class ActivityStreamsTest
    {
        [TestMethod]
        public void TestAllTimeRecordsExistNoGap()
        {
            long activityId = 1234L;

            ActivityStreams s = ActivityStreams.CreateForNewActivity(activityId);

            // check activityId correctly assigned.
            Assert.AreEqual(s.ActivityId, activityId);

            int itemsAdded = 0;

            for (int x = 0; x < 100; x++)
            {
                s.Stream.Add(new Stream() { Time = x });
                itemsAdded++;
            }

            Assert.AreEqual(s.Stream.Count(), itemsAdded);
            Assert.AreEqual(false, s.GapsInStream());
        }

        [TestMethod]
        public void TestAllTimeRecordsExistWithGap()
        {
         
            long activityId = 1234L;

            ActivityStreams stream = ActivityStreams.CreateForNewActivity(activityId);

            // check activityId correctly assigned.
            Assert.AreEqual(stream.ActivityId, activityId);

            int itemsAdded = 0;
            // add a block of 100.
            for (int x = 0; x < 100; x++)
            {
                stream.Stream.Add(new Stream() { Time = x });
                itemsAdded++;
            }

            // and another block but leaving a gap of missing time records.
            for (int x = 105; x < 200; x++)
            {
                stream.Stream.Add(new Stream() { Time = x });
                itemsAdded++;
            }

            Assert.AreEqual(stream.Stream.Count(), itemsAdded);

            // check we are picking up that there is a gap in the stream.
            Assert.AreEqual(true, stream.GapsInStream());

            // fix the gap.
            stream.FixGaps();

            // and check it's now reporting no gaps.
            Assert.AreEqual(false, stream.GapsInStream());


            // pull out one of the added stream items and check it exists and time/activityId are correct.
            Stream added = stream.Stream.Where(s => s.Time == 101).FirstOrDefault();

            Assert.IsNotNull(added);

            Assert.AreEqual(101, added.Time);
            Assert.AreEqual(activityId, added.ActivityId);





        }
    }
}