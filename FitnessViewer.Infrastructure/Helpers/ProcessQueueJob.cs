using FitnessViewer.Infrastructure.Data;
using FitnessViewer.Infrastructure.Models;
using FitnessViewer.Infrastructure.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessViewer.Infrastructure.Helpers
{
    public class ProcessQueueJob
    {
        private UnitOfWork _uow;
        private int _jobId;
        private DownloadQueue _jobDetails;

        public ProcessQueueJob(int jobId)
        {
            _uow = new UnitOfWork();
            _jobId = jobId;
      
        }

        public  bool IsJobValid()
        {
            _jobDetails = _uow.Queue.Find(_jobId);

            if (_jobDetails == null)
                return false;

            if (_jobDetails.Processed)
                return false;

            if (_jobDetails.HasError.HasValue)
                if (_jobDetails.HasError.Value)
                    return false;

            return true;                    
        }


        public  void ProcessJob()
        {
            try
            {
                if (_jobDetails.DownloadType == DownloadType.Strava)
                    StravaDownload();
                else if (_jobDetails.DownloadType == DownloadType.Fitbit)
                    FitbitDownload();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                _jobDetails.JobHasError();
                _uow.Complete();
            }
        }

        private  void FitbitDownload()
        {

            FitbitHelper fitbit = new FitbitHelper(_uow, _jobDetails.UserId);
            fitbit.Download(false);
            _jobDetails.MarkJobComplete();
            _uow.Complete();

        }

        private  void StravaDownload()
        {

            if (_jobDetails.ActivityId != null)
            {
                // download full details for an individual activity.
                StravaActivityDownload s = new StravaActivityDownload(_uow, _jobDetails.UserId);
                s.ActivityDetailsDownload(_jobDetails.ActivityId.Value);
                _jobDetails.MarkJobComplete();
                _uow.Complete();
            }
            else
            {
                // if _jobDetails isn't for an individual activity then it must be a request to search for new activities.
                StravaActivityScan s = new StravaActivityScan(_uow, _jobDetails.UserId);
                s.AddActivitesForAthlete();
                _jobDetails.MarkJobComplete();
                _uow.Complete();
            }
        }


    }
}
