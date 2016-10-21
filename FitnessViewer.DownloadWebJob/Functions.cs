using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using FitnessViewer.Infrastructure.Helpers;
using AutoMapper;
using static FitnessViewer.Infrastructure.Configuration.AutoMapperConfig;

namespace FitnessViewer.DownloadWebJob
{
    public class Functions
    {
        // This function will get triggered/executed when a new message is written 
        // on an Azure Queue called queue.
        public static void ProcessQueueMessage([QueueTrigger("fitness-viewer-download-queue")] string message, TextWriter log)
        {
       

          Console.WriteLine(string.Format("Processing: {0}", message));

            AutoMapperConfig();

            ProcessQueueJob job = new ProcessQueueJob(Convert.ToInt32(message));
            if (!job.IsJobValid())
            {
                log.WriteLine(string.Format("Invalid job: {0}", message));
                return;
            }

            job.ProcessJob();

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
