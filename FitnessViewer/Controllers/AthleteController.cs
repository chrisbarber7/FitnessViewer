﻿using FitnessViewer.Infrastructure.enums;
using FitnessViewer.Infrastructure.Helpers;
using FitnessViewer.Infrastructure.Repository;
using FitnessViewer.ViewModels;
using Microsoft.AspNet.Identity;
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

            var result = new AthleteViewModel()
            {
                FirstName = _unitOfWork.Athlete.FindAthleteByUserId(this.User.Identity.GetUserId()).FirstName,
                PowerPeaks = _unitOfWork.Analysis.GetPeaks(userId, PeakStreamType.Power),
                RunningTime = _unitOfWork.Activity.GetBestTimes(userId),
                CurrentWeight = _unitOfWork.Metrics.GetWeightDetails(userId, 1)[0]
            };

            return View(result);
        }

        public ActionResult ActivityScan()
        {
            _unitOfWork.Queue.AddQueueItem(this.User.Identity.GetUserId(), DownloadType.Strava);
            _unitOfWork.Complete();

            // trigger web job to download activity details.
            AzureWebJob.CreateTrigger();

            return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri);
        }
    }
}

