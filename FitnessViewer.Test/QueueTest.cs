using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitnessViewer.Infrastructure.Data;
using FitnessViewer.Infrastructure.Models;

namespace FitnessViewer.Test
{
    [TestClass]
    public class QueueTest
    {


        [AssemblyInitialize()]
        public static void AssemblyInit(TestContext context)
        {
            System.Diagnostics.Debug.WriteLine("Assembly Init");
        }

        [ClassInitialize()]
        public static void ClassInit(TestContext context)
        {
            System.Diagnostics.Debug.WriteLine("ClassInit");
        }

        [TestInitialize()]
        public void Initialize()
        {
            System.Diagnostics.Debug.WriteLine("TestMethodInit");
        }

        [TestCleanup()]
        public void Cleanup()
        {
            System.Diagnostics.Debug.WriteLine("TestMethodCleanup");
        }

        [ClassCleanup()]
        public static void ClassCleanup()
        {
            System.Diagnostics.Debug.WriteLine("ClassCleanup");
        }

        [AssemblyCleanup()]
        public static void AssemblyCleanup()
        {
            System.Diagnostics.Debug.WriteLine("AssemblyCleanup");
        }

        [TestMethod]
        public void AddToQueue()
        {
            Repository repo = new Repository();
            string userId = "TestUserId";
            repo.AddQueueItem(userId);

  
            var queue = repo.FindQueueItemByUserId(userId);
              

        }

        [TestMethod]
        public void Test2()
            {
            Assert.AreEqual(2, 2);
            }
    }
}


