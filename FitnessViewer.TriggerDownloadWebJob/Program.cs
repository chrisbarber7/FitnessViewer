using FitnessViewer.Infrastructure.Data;
using FitnessViewer.Infrastructure.Interfaces;
using FitnessViewer.Infrastructure.Models;

namespace FitnessViewer.TriggerDownloadWebJob
{
    // To learn more about Microsoft Azure WebJobs SDK, please see http://go.microsoft.com/fwlink/?LinkID=320976
    class Program
    {
        // Please set the following connection strings in app.config for this WebJob to run:
        // AzureWebJobsDashboard and AzureWebJobsStorage
        static void Main()
        {
            IUnitOfWork uow = new UnitOfWork();
            var athletes = uow.CRUDRepository.GetAll<Athlete>();

            foreach (Athlete a in athletes)
                DownloadQueue.CreateQueueJob(a.UserId, Infrastructure.enums.DownloadType.Strava).Save();

            var fitbitUsers = uow.CRUDRepository.GetAll<FitbitUser>();

            foreach (FitbitUser fb in fitbitUsers)
                DownloadQueue.CreateQueueJob(fb.UserId, Infrastructure.enums.DownloadType.Fitbit).Save();
        }
    }
}
