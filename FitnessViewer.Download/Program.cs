using FitnessViewer.Infrastructure.Repository;
using FitnessViewer.Infrastructure.Models;
using FitnessViewer.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using static FitnessViewer.Infrastructure.Configuration.AutoMapperConfig;
using FitnessViewer.Infrastructure.Data;
using System.IO;
using FitnessViewer.Infrastructure.enums;

namespace FitnessViewer.Download
{
    class Program
    {
        static void Main(string[] args)
        {
            AutoMapperConfig();

            UnitOfWork _unitOfWork = new Infrastructure.Data.UnitOfWork();

            //string userId = "e0113fcc-7546-4c88-872f-c27e196c4d5c";

            //var activities = _unitOfWork.Activity.GetActivities(userId);

            //foreach (Activity a in activities)
            //{
            //    DownloadQueue job = DownloadQueue.CreateQueueJob(userId, DownloadType.Strava, a.Id);
            //    _unitOfWork.Queue.AddQueueItem(job);
            //    _unitOfWork.Complete();
            //    AzureWebJob.AddToAzureQueue(job.Id);

            //StreamHelper.RecalculateAllActivities(_unitOfWork);
            //}

            while (true)
            {
                var jobs = _unitOfWork.Queue.GetQueue(20);

                if (jobs.Count() == 0)
                    break;

               ProcessJobs(_unitOfWork, jobs);
            }
        }

        /// <summary>
        /// Process jobs in queue
        /// </summary>
        /// <param name="uow">UnitOfWork</param>
        /// <param name="jobs">list of jobs to process</param>
        private static void ProcessJobs(UnitOfWork uow, IEnumerable<DownloadQueue> jobs)
        {
            foreach (DownloadQueue job in jobs)
            {
                if (job.DownloadType == Infrastructure.enums.DownloadType.Invalid)
                {
                    job.JobHasError();
                    uow.Complete();
                    continue;
                }

                // if previously processed and had an error skip it
                if (job.HasError != null)
                    if (job.HasError.Value)
                        continue;

                ProcessQueueJob queueJob = new ProcessQueueJob(job.Id);
                if (!queueJob.IsJobValid())
                {
                    job.JobHasError();
                    uow.Complete();
                    continue;
                }

                queueJob.ProcessJob();

            }
        }
        

        /// <summary>
        /// Setup automapper
        /// </summary>
        private static void AutoMapperConfig()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<InfrasturtureProfile>();
            });
        }

    }
}
