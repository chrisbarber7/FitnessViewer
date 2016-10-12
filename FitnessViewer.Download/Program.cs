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

                ProrcessJobs(_unitOfWork, jobs);
            }
        }

        /// <summary>
        /// Process jobs in queue
        /// </summary>
        /// <param name="uow">UnitOfWork</param>
        /// <param name="jobs">list of jobs to process</param>
        private static void ProrcessJobs(UnitOfWork uow, IEnumerable<DownloadQueue> jobs)
        {
            foreach (DownloadQueue job in jobs)
                ProcessJob(uow, job);
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
                // if previously processed and had an error skip it
                if (job.HasError != null)
                    if (job.HasError.Value)
                        return;

                if (job.ActivityId != null)
                {
                    StravaActivityDownload s = new StravaActivityDownload(job.UserId);
                    s.ActivityDetailsDownload(job.ActivityId.Value);
                    uow.Queue.RemoveQueueItem(job.Id);
                    uow.Complete();
                }
                else
                { 
                    // if job isn't for an individual activity then it must be a request to search for new activities.
                    StravaActivityScan s = new StravaActivityScan(job.UserId);
                    s.AddActivitesForAthlete();
                    uow.Queue.RemoveQueueItem(job.Id);
                    uow.Complete();
                }
           
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
