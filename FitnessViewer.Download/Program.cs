using FitnessViewer.Infrastructure.Data;
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

            foreach (StravaQueue job in jobs)
            {
                if (job.Activity == null)
                {
                    Strava s = new Strava(job.UserId);
                    s.AddActivitesForAthlete();

                    _repo.RemoveQueueItem(job.Id);
                }
                else
                {
                    Strava s = new Strava(job.UserId);
                    s.ActivityDetailsDownload(job.StravaActivityId.Value);
                    _repo.RemoveQueueItem(job.Id);
                }


            }

        }
    }
}
