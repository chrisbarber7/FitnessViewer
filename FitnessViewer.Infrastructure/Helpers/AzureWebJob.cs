using FitnessViewer.Infrastructure.Data;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace FitnessViewer.Infrastructure.Helpers
{
    /// <summary>
    /// Helper to trigger starting of download web app.
    /// </summary>
    public class AzureWebJob
    {
        /// <summary>
        /// Method to add a job to the azure queue which will be processed by the continually running WebJob (FitnessViewer.DownloadWebJob)
        /// </summary>
        /// <param name="jobId"></param>
        public static void AddToAzureQueue(int jobId)
        {
            try
            {
                if (ConfigurationManager.AppSettings["useAzureQueue"].ToLower() == "false")
                    return;

                // get storage account from connection string (held in WebApp settings/connection strings)
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                        ConfigurationManager.ConnectionStrings["AzureWebJobsStorage"].ConnectionString);

                // Create a queue client
                CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();

                // Retrieve a reference to a queue
                CloudQueue queue = queueClient.GetQueueReference("fitness-viewer-download-queue");

                // Create the queue if it doesn’t already exist (it should exists)
                queue.CreateIfNotExists();

                // Pcreate a message and add it to the queue.
                CloudQueueMessage message = new CloudQueueMessage(jobId.ToString());

                // add message to the queue
                queue.AddMessage(message);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                
            }
        }
    }
}

