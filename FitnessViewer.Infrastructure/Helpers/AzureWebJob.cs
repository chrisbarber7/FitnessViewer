using FitnessViewer.Infrastructure.Data;
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
        private int _attempts = 0;
        private UnitOfWork _unitOfWork ;

        private AzureWebJob(UnitOfWork uow)
        {
            _unitOfWork = uow;
        }

        public static void CreateTrigger(UnitOfWork uow)
        {
            if (ConfigurationManager.AppSettings["AzureWebJobTriggerEnabled"].ToUpper() != "TRUE")
                   return;

             new AzureWebJob(uow).TriggerJob();
        }

        /// <summary>
        /// Send Post request to azure to start the download job.
        /// </summary>
        private void TriggerJob()
        {
            AzureWebJobStatus status = AzureWebJobStatus.Invalid;

            // have five attempts at starting the job
            while ((status != AzureWebJobStatus.Started) && (_attempts <= 5))
            {
                try
                {
                    System.Diagnostics.Debug.WriteLine(string.Format("Triggering Job (attempt:{0})", _attempts.ToString()));
                    status = Post();
                }
                catch (Exception)
                {
                    status = AzureWebJobStatus.Error;
                }

                System.Diagnostics.Debug.WriteLine(string.Format("Attempt: {0} Result: {1}", _attempts.ToString(), status.ToString()));

                // if web job is already running and no jobs outstanding then no need to retry.
                if (status == AzureWebJobStatus.AlreadyRunning)
                    if (!OutstandingJobs())
                        status = AzureWebJobStatus.Started;

                _attempts++;
            }
        }

        /// <summary>
        /// Post method
        /// </summary>
        /// <returns></returns>
        private AzureWebJobStatus Post()
        {
            string url = ConfigurationManager.AppSettings["AzureDownloadWebJobUrl"];
            string username = ConfigurationManager.AppSettings["AzureDownloadWebJobUsername"];
            string password = ConfigurationManager.AppSettings["AzureDownloadWebJobPassword"];

            var handler = new HttpClientHandler();
            var httpClient = new HttpClient(handler);

            // not sure why but need to include login details twice to get authorisation to work? 
            httpClient.DefaultRequestHeaders.Authorization =
                         new AuthenticationHeaderValue("Basic",
                                                        Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(string.Format("{0}:{1}", username, password))));

            var values = new Dictionary<string, string> { { "username", username }, { "password", password } };
      
            HttpContent header = new FormUrlEncodedContent(values);
            var response = httpClient.PostAsync(url, header).Result;

            if (response.IsSuccessStatusCode)
                return AzureWebJobStatus.Started;
            else if (response.StatusCode == HttpStatusCode.Conflict)
                return AzureWebJobStatus.AlreadyRunning;
            else
                return AzureWebJobStatus.Error;
         }

        /// <summary>
        /// return whether or not the download queue has jobs outstanding.
        /// </summary>
        /// <returns></returns>
        private bool OutstandingJobs()
        {
            if (_unitOfWork.Queue.GetQueueCount() == 0)
                return false;

            return true;
        }

        // status of post
        private enum AzureWebJobStatus
        {
            Invalid,
            Started,
            AlreadyRunning,
            Error
        }
    }
}

