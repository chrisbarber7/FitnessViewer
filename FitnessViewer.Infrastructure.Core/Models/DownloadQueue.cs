using FitnessViewer.Infrastructure.Core.Data;
using FitnessViewer.Infrastructure.Core.enums;
using FitnessViewer.Infrastructure.Core.Helpers;
using FitnessViewer.Infrastructure.Core.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessViewer.Infrastructure.Core.Models
{
    public class DownloadQueue : Entity<int>, IEntity<int>, IUserEntity
    {
        private DownloadQueue()
        { }

       // public  int Id { get; private set; }

        [Required]
        [MaxLength(450)]
        [ForeignKey("User")]
        public string UserId { get;  set; }
        public virtual ApplicationUser User { get; set; }

        public DateTime Added { get; private set; }
        public bool Processed { get; set; }
        public DateTime? ProcessedAt { get; set; }



        public long? ActivityId { get; private set; }
        public bool? HasError { get; set; }
        public DownloadType DownloadType { get; set; }
        public int? Duration { get; set; }

        internal static DownloadQueue CreateQueueJob(DownloadQueue job)
        {
            return CreateQueueJob(job.UserId, job.DownloadType, job.ActivityId, job.Duration);
        }

        public static DownloadQueue CreateQueueJob(string userId, DownloadType type)
        {
            return DownloadQueue.CreateQueueJob(userId, type, null);
        }

        public static DownloadQueue CreateQueueJob(string userId, DownloadType type, long? activityId)
        {
                return CreateQueueJob(userId, type, activityId, null);
        }

        /// <summary>
        /// Create a new job for the queue.
        /// </summary>
        /// <param name="userId">ASP.NET Identity User Id</param>
        /// <param name="activityId">Optional Strava activity id</param>
        /// <returns></returns>
        public static DownloadQueue CreateQueueJob(string userId, DownloadType type, long? activityId, int? duration)
        {
            DownloadQueue q = new DownloadQueue();
            q.UserId = userId;
            q.DownloadType = type;
            q.ActivityId = activityId;

            q.Added = DateTime.Now;
            q.Processed = false;
            q.ProcessedAt = null;
            q.HasError = false;
            q.Duration = duration;
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

        //public void AddToAzureQueue()
        //{
        //    AzureWebJob.AddToAzureQueue(Id);
        //}

        public void Save()
        {
            this.Save(null);
        }

        public void Save(IUnitOfWork uow)
        {
            if (uow == null)
                uow = new Data.UnitOfWork();

            uow.CRUDRepository.Add<DownloadQueue>(this);
         //   uow.Queue.AddQueueItem(this);
            uow.Complete();
         //   AddToAzureQueue();
        }
    }
}

