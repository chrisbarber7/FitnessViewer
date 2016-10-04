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

namespace FitnessViewer.Download
{
    class Program
    {
        static void Main(string[] args)
        {
            AutoMapperConfig();
            
       Infrastructure.Data.UnitOfWork _unitOfWork = new Infrastructure.Data.UnitOfWork();

          // un-comment to force recalculation of peak information from stream table.
          //  StreamHelper.RecalculateAllActivities();
          //  StreamHelper.RecalculateSingleActivity(18698122);

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
                        StravaActivityScan s = new StravaActivityScan(job.UserId);
                        s.AddActivitesForAthlete();
                        _unitOfWork.Queue.RemoveQueueItem(job.Id);
                        _unitOfWork.Complete();
                    }
                    else
                    {
                        StravaActivityDownload s = new StravaActivityDownload(job.UserId);
                        s.ActivityDetailsDownload(job.ActivityId.Value);
                        _unitOfWork.Queue.RemoveQueueItem(job.Id);
                        _unitOfWork.Complete();
                    }
                }
                catch (Exception ex)
                {
                    _unitOfWork.Queue.QueueItemMarkHasError(job.Id);
                    _unitOfWork.Complete();
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
                finally
                {

                }
            }
        }

        private static void AutoMapperConfig()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<InfrasturtureProfile>();
            });
        }

    }
}
