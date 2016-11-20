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
using FitnessViewer.Infrastructure.Models.Collections;
using FitnessViewer.Infrastructure.Interfaces;

namespace FitnessViewer.Download
{
    class Program
    {
        static void Main(string[] args)
        {
            AutoMapperConfig();

            Infrastructure.Interfaces.IUnitOfWork _unitOfWork = new Infrastructure.Data.UnitOfWork();

            var userId = "e0113fcc-7546-4c88-872f-c27e196c4d5c";
            //       var userId2 = "b2b41546-7248-4fcc-bf6a-694672c7d0cc";


            var metrics = _unitOfWork.CRUDRepository.GetByUserId<Metric>(userId)
                .Where(x=>x.MetricType == MetricType.Weight).ToList();




    var a =         _unitOfWork.CRUDRepository.GetByKey<Activity>(4395245L, o=>o.ActivityType);

            Metric m = Metric.CreateMetric(userId, MetricType.Weight, DateTime.Now, 1.23M, false);

            _unitOfWork.CRUDRepository.Add<Metric>(m);
            _unitOfWork.Complete();

  //          FitbitHelper h = new FitbitHelper(_unitOfWork, userId);

      
           //  h.Download(false);
       //     h.Download(true);

          //  StravaUpdate upd = new StravaUpdate(_unitOfWork, userId2);
          //  upd.UpdatePrivateFlag(774380153);

   //         ActivityWeight w1 = new ActivityWeight(userId);
   //         w1.UpdateActivityWeight();



            ActivityWeight w = new ActivityWeight(userId, 771368208);
            decimal? we = w.GetActivityWeight();



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
        private static void ProcessJobs(Infrastructure.Interfaces.IUnitOfWork uow, IEnumerable<DownloadQueue> jobs)
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
