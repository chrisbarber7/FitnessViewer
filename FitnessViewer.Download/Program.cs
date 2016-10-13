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

namespace FitnessViewer.Download
{
    class Program
    {
        static void Main(string[] args)
        {
            AutoMapperConfig();

            UnitOfWork _unitOfWork = new Infrastructure.Data.UnitOfWork();

            // un-comment to force recalculation of peak information from stream table.
            //     StreamHelper.RecalculateAllActivities();
            //    StreamHelper.RecalculateSingleActivity(95887633);

            while (true)
            {
                var jobs = _unitOfWork.Queue.GetQueue();

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
                    uow.Queue.QueueItemMarkHasError(job.Id);
                    uow.Complete();
                    continue;
                }

                // if previously processed and had an error skip it
                if (job.HasError != null)
                    if (job.HasError.Value)
                        continue;

                ProcessJob(uow, job);
            }
        }

        /// <summary>
        /// Process individual job.
        /// </summary>
        /// <param name="uow"></param>
        /// <param name="job"></param>
        private static void ProcessJob(UnitOfWork uow, DownloadQueue job)
        {
            try
            {
                if (job.DownloadType == Infrastructure.enums.DownloadType.Strava)
                    StravaDownload(uow, job);
                else if (job.DownloadType == Infrastructure.enums.DownloadType.Fitbit)
                    FitbitDownload(uow, job);
            }
            catch (Exception ex)
            {
                uow.Queue.QueueItemMarkHasError(job.Id);
                uow.Complete();
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {

            }
        }

        private static void FitbitDownload(UnitOfWork uow, DownloadQueue job)
        {
            try
            {
                FitbitHelper fitbit = new FitbitHelper(uow, job.UserId);
                fitbit.Download(false);
                uow.Queue.RemoveQueueItem(job.Id);
                uow.Complete();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                uow.Queue.QueueItemMarkHasError(job.Id);
                uow.Complete();
            }
        }

        private static void StravaDownload(UnitOfWork uow, DownloadQueue job)
        {
            if (job.ActivityId != null)
            {
                // download full details for an individual activity.
                StravaActivityDownload s = new StravaActivityDownload(uow, job.UserId);
                s.ActivityDetailsDownload(job.ActivityId.Value);
                uow.Queue.RemoveQueueItem(job.Id);
                uow.Complete();
            }
            else
            {
                // if job isn't for an individual activity then it must be a request to search for new activities.
                StravaActivityScan s = new StravaActivityScan(uow, job.UserId);
                s.AddActivitesForAthlete();
                uow.Queue.RemoveQueueItem(job.Id);
                uow.Complete();
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
