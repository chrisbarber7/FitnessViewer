using FitnessViewer.Infrastructure.Data;
using FitnessViewer.Infrastructure.Models;
using FitnessViewer.Infrastructure.enums;
using System;

using FitnessViewer.Infrastructure.Models.Collections;
using FitnessViewer.Infrastructure.Interfaces;

namespace FitnessViewer.Infrastructure.Helpers
{
    public class ProcessQueueJob
    {
        private Interfaces.IUnitOfWork _uow;
        private int _jobId;
        private DownloadQueue _jobDetails;

        public ProcessQueueJob(int jobId)
        {
            _uow = new Data.UnitOfWork();
            _jobId = jobId;
        }

        public void ResumbitJob()
        {
            _jobDetails = _uow.CRUDRepository.GetByKey<DownloadQueue>(_jobId);

            if (_jobDetails == null)
                return;

             DownloadQueue.CreateQueueJob(_jobDetails).Save();
        }

        public  bool IsJobValid()
        {
            _jobDetails = _uow.CRUDRepository.GetByKey<DownloadQueue>(_jobId);

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
            Console.WriteLine(string.Format("Processing Job: {0} ActivityId:{1}", _jobDetails.Id, _jobDetails.ActivityId));
            try
            {
                if (_jobDetails.DownloadType == DownloadType.Strava)
                    StravaDownload();
                else if (_jobDetails.DownloadType == DownloadType.Fitbit)
                    FitbitDownload();
                else if (_jobDetails.DownloadType == DownloadType.CalculateActivityStats)
                    CalculateStats();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                Console.WriteLine(ex.Message);
                _jobDetails.JobHasError();
                _uow.Complete();
            }
            Console.WriteLine(string.Format("Finished Processing Job: {0} ActivityId:{1}", _jobDetails.Id, _jobDetails.ActivityId));
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

        private void CalculateStats()
        {

         ActivityStreams.CreateFromExistingActivityStream(_jobDetails.ActivityId.Value)
                .CalculatePeak(StreamType.Watts, _jobDetails.Duration)
                .Save(false);

            _jobDetails.MarkJobComplete();
            _uow.Complete();
        }
    }
}
