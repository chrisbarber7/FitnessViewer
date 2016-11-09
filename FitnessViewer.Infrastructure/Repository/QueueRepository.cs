using FitnessViewer.Infrastructure.Data;
using FitnessViewer.Infrastructure.Models;
using System.Collections.Generic;
using System.Linq;

namespace FitnessViewer.Infrastructure.Repository
{
    public class QueueRepository
    {
        private ApplicationDb _context;

        public QueueRepository(ApplicationDb context)
        {
            _context = context;
        }

        #region Queue


        public void AddQueueItem(DownloadQueue newJob)
        {
            _context.Queue.Add(newJob);
        }

        internal DownloadQueue Find(int jobId)
        {
            return _context.Queue.Find(jobId);
        }

        public IEnumerable<DownloadQueue> GetQueue(int count)
        {
            return _context.Queue.Where(x => !x.Processed && !x.HasError.Value).OrderBy(x=>x.Id).Take(count).ToList();
        }

        public IEnumerable<DownloadQueue> GetFailedJob()
        {
            return _context.Queue
                           .Where(x => x.HasError.Value)
                           .OrderBy(x => x.Id)
                           .ToList();
        }


        ///// <summary>
        ///// Queue Count
        ///// </summary>
        ///// <returns>Number of items in queue waiting to be processed</returns>
        //public int GetQueueCount()
        //{
        //    return _context.Queue.Where(x => !x.Processed && !x.HasError.Value).Count();
        //}

    


        //public IEnumerable<DownloadQueue> FindQueueItemByUserId(string userId)
        //{
        //    return _context.Queue.Where(x => x.UserId == userId).ToList();
        //}
        #endregion
    }
}
