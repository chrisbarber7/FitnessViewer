using FitnessViewer.Infrastructure.Repository;
using FitnessViewer.Infrastructure.Models;
using FitnessViewer.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessViewer.Download
{
    class Program
    {
        static void Main(string[] args)
        {
            Infrastructure.Data.UnitOfWork _unitOfWork = new Infrastructure.Data.UnitOfWork();

        var jobs = _unitOfWork.Queue.GetQueue();

            foreach (DownloadQueue job in jobs)
            {
                try
                {
                    if (job.HasError != null)
                        if (job.HasError.Value)
                            continue;

                    if (job.ActivityId == null)
                    {
                        Strava s = new Strava(job.UserId);
                        s.AddActivitesForAthlete();
                        _unitOfWork.Queue.RemoveQueueItem(job.Id);
                    }
                    else
                    {
                        Strava s = new Strava(job.UserId);
                        s.ActivityDetailsDownload(job.ActivityId.Value);
                        _unitOfWork.Queue.RemoveQueueItem(job.Id);
                    }
                }
                catch (Exception ex)
                {
                    _unitOfWork.Queue.QueueItemMarkHasError(job.Id);
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            }
        }
    }
}
