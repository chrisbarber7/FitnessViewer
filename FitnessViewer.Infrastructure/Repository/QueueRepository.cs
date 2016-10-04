﻿using FitnessViewer.Infrastructure.Data;
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
        public void AddQueueItem(string userId)
        {
            this.AddQueueItem(userId, null);
        }

        public void AddQueueItem(string userId, long? activityId)
        {
            DownloadQueue q = new DownloadQueue() { UserId = userId, Added = DateTime.Now, Processed = false, ActivityId = activityId };
            _context.Queue.Add(q);
        }

        public IEnumerable<DownloadQueue> GetQueue()
        {
            return _context.Queue.Where(x => !x.Processed).OrderBy(x=>x.Id).ToList();
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