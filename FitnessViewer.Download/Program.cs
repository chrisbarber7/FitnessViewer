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
            Repository _repo = new Repository();
            
            var jobs = _repo.GetQueue();

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
                        _repo.RemoveQueueItem(job.Id);
                    }
                    else
                    {
                        Strava s = new Strava(job.UserId);
                        s.ActivityDetailsDownload(job.ActivityId.Value);
                        _repo.RemoveQueueItem(job.Id);
                    }
                }
                catch (Exception ex)
                {
                    _repo.QueueItemMarkHasError(job.Id);
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            }
        }
    }
}
