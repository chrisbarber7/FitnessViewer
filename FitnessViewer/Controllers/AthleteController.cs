using FitnessViewer.Infrastructure.enums;
using FitnessViewer.Infrastructure.Helpers;
using FitnessViewer.Infrastructure.Models;
using FitnessViewer.Infrastructure.Models.Dto;
using FitnessViewer.Infrastructure.Repository;
using FitnessViewer.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Web.Mvc;

namespace FitnessViewer.Controllers
{
    [Authorize]
    public class AthleteController : Controller
    {
        private Infrastructure.Data.UnitOfWork _unitOfWork;

        public AthleteController()
        {

            _unitOfWork = new Infrastructure.Data.UnitOfWork();
        }

        public ActionResult Dashboard()
        {
            var userId = this.User.Identity.GetUserId();

            SportSummaryDto run7DayStats = _unitOfWork.Activity.GetSportSummary(this.User.Identity.GetUserId(), "Run", DateTime.Now.AddDays(-7), DateTime.Now);
            SportSummaryDto bike7DayStats = _unitOfWork.Activity.GetSportSummary(this.User.Identity.GetUserId(), "Ride", DateTime.Now.AddDays(-7), DateTime.Now);
            SportSummaryDto swim7DayStats = _unitOfWork.Activity.GetSportSummary(this.User.Identity.GetUserId(), "Swim", DateTime.Now.AddDays(-7), DateTime.Now);
            SportSummaryDto other7DayStats = _unitOfWork.Activity.GetSportSummary(this.User.Identity.GetUserId(), "Other", DateTime.Now.AddDays(-7), DateTime.Now);
            SportSummaryDto all7DayStats = _unitOfWork.Activity.GetSportSummary(this.User.Identity.GetUserId(), "All", DateTime.Now.AddDays(-7), DateTime.Now);


            SportSummaryDto run30DayStats = _unitOfWork.Activity.GetSportSummary(this.User.Identity.GetUserId(), "Run", DateTime.Now.AddDays(-30), DateTime.Now);
            SportSummaryDto bike30DayStats = _unitOfWork.Activity.GetSportSummary(this.User.Identity.GetUserId(), "Ride", DateTime.Now.AddDays(-30), DateTime.Now);
            SportSummaryDto swim30DayStats = _unitOfWork.Activity.GetSportSummary(this.User.Identity.GetUserId(), "Swim", DateTime.Now.AddDays(-30), DateTime.Now);
            SportSummaryDto other30DayStats = _unitOfWork.Activity.GetSportSummary(this.User.Identity.GetUserId(), "Other", DateTime.Now.AddDays(-30), DateTime.Now);
            SportSummaryDto all30DayStats = _unitOfWork.Activity.GetSportSummary(this.User.Identity.GetUserId(), "All", DateTime.Now.AddDays(-30), DateTime.Now);


            var result = new AthleteViewModel()
            {
                FirstName = _unitOfWork.Athlete.FindAthleteByUserId(this.User.Identity.GetUserId()).FirstName,
                PowerPeaks = _unitOfWork.Analysis.GetPeaks(userId, PeakStreamType.Power),
                RunningTime = _unitOfWork.Activity.GetBestTimes(userId),
                CurrentWeight = _unitOfWork.Metrics.GetWeightDetails(userId, 1)[0],
                RecentActivity = _unitOfWork.Activity.GetActivityDto(userId, 7),
                Run7Day = run7DayStats,
                Bike7Day = bike7DayStats,
                Swim7Day = swim7DayStats,
                Other7Day = other7DayStats,
                All7Day = all7DayStats,
                Run30Day = run30DayStats,
                Bike30Day = bike30DayStats,
                Swim30Day = swim30DayStats,
                Other30Day = other30DayStats,
                All30Day = all30DayStats,
            };

            return View(result);
        }

        public ActionResult ActivityScan()
        {



            DownloadQueue job = DownloadQueue.CreateQueueJob(this.User.Identity.GetUserId(), DownloadType.Strava);
            _unitOfWork.Queue.AddQueueItem(job);
            _unitOfWork.Complete();
            AzureWebJob.AddToAzureQueue(job.Id);

            // trigger web job to download activity details.
            AzureWebJob.CreateTrigger(_unitOfWork);

            return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri);
        }
    }
}

