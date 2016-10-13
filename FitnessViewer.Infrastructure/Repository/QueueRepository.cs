using FitnessViewer.Infrastructure.Data;
using FitnessViewer.Infrastructure.enums;
using FitnessViewer.Infrastructure.Models;
using System;
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

        public void AddQueueItem(string userId, DownloadType type)
        {
            this.AddQueueItem(userId, type, null);
        }

        public void AddQueueItem(string userId, DownloadType type, long? activityId)
        {
            _context.Queue.Add(DownloadQueue.CreateQueueJob(userId, type, activityId));
        }

        public IEnumerable<DownloadQueue> GetQueue()
        {
            return _context.Queue.Where(x => !x.Processed && !x.HasError.Value).OrderBy(x=>x.Id).ToList();
        }

        public void RemoveQueueItem(int id)
        {
            DownloadQueue q = _context.Queue.Find(id);
            q.Processed = true;
            q.ProcessedAt = DateTime.Now;
            q.HasError = false;
        }

        public void QueueItemMarkHasError(int id)
        {
            DownloadQueue q = _context.Queue.Find(id);
            q.HasError = true;
        }

        public IEnumerable<DownloadQueue> FindQueueItemByUserId(string userId)
        {
            return _context.Queue.Where(x => x.UserId == userId).ToList();
        }
        #endregion
    }
}
