using FitnessViewer.Infrastructure.Core.Repository;
using FitnessViewer.Infrastructure.Core.Models;
using FitnessViewer.Infrastructure.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
//using static FitnessViewer.Infrastructure.Core.Configuration.AutoMapperConfig;
using FitnessViewer.Infrastructure.Core.Data;
using System.IO;
using FitnessViewer.Infrastructure.Core.enums;
using FitnessViewer.Infrastructure.Core.Models.Collections;
using FitnessViewer.Infrastructure.Core.Interfaces;
using FitnessViewer.Infrastructure.Core.Helpers.Analytics;

namespace FitnessViewer.Download.Core
{
    class Program
    {
        static void Main(string[] args)
        {

    //        AutoMapperConfig();

            Infrastructure.Core.Interfaces.IUnitOfWork _unitOfWork = new Infrastructure.Core.Data.UnitOfWork();

            int? type = null;

            if (args.Count() >= 1)
            {
                type = Convert.ToInt32(args[0]);
            }

            while (true)
            {
                var jobs = _unitOfWork.Queue.GetQueue(20, type);

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
        private static void ProcessJobs(Infrastructure.Core.Interfaces.IUnitOfWork uow, IEnumerable<DownloadQueue> jobs)
        {
            foreach (DownloadQueue job in jobs)
            {
                if (job.DownloadType == Infrastructure.Core.enums.DownloadType.Invalid)
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
        //private static void AutoMapperConfig()
        //{
        //    Mapper.Initialize(cfg =>
        //    {
        //        cfg.AddProfile<InfrasturtureProfile>();
        //    });
        //}

    }
}
