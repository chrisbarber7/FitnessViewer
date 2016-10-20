﻿using FitnessViewer.Infrastructure.Data;
using FitnessViewer.Infrastructure.enums;
using FitnessViewer.Infrastructure.Helpers;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessViewer.Infrastructure.Models
{
    public class DownloadQueue
    {
        private DownloadQueue()
        { }

        public int Id { get; private set; }

        [Required]
        [MaxLength(128)]
        [ForeignKey("User")]
        public string UserId { get; private set; }
        public virtual ApplicationUser User { get; set; }

        public DateTime Added { get; private set; }
        public bool Processed { get; set; }
        public DateTime? ProcessedAt { get; set; }
        public long? ActivityId { get; private set; }
        public bool? HasError { get; set; }
        public DownloadType DownloadType { get; set; }


        public static DownloadQueue CreateQueueJob(string userId, DownloadType type)
        {
            return DownloadQueue.CreateQueueJob(userId, type, null);
        }

        /// <summary>
        /// Create a new job for the queue.
        /// </summary>
        /// <param name="userId">ASP.NET Identity User Id</param>
        /// <param name="activityId">Optional Strava activity id</param>
        /// <returns></returns>
        public static DownloadQueue CreateQueueJob(string userId, DownloadType type, long? activityId)
        {
            DownloadQueue q = new DownloadQueue();
            q.UserId = userId;
            q.DownloadType = type;
            q.ActivityId = activityId;

            q.Added = DateTime.Now;
            q.Processed = false;
            q.ProcessedAt = null;
            q.HasError = false;

            return q;
        }

        public void JobHasError()
        {
            HasError = true;
        }

        public void MarkJobComplete()
        {
            Processed = true;
            ProcessedAt = DateTime.Now;
            HasError = false;
        }

        public void AddToAzureQueue()
        {
            AzureWebJob.AddToAzureQueue(Id);
        }
    }
}

