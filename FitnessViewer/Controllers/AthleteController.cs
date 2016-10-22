﻿using FitnessViewer.Infrastructure.enums;
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

            SportSummaryDto summ = _unitOfWork.Activity.GetSportSummary(this.User.Identity.GetUserId(), "Run", DateTime.Now.AddDays(-7), DateTime.Now);


            var result = new AthleteViewModel()
            {
                FirstName = _unitOfWork.Athlete.FindAthleteByUserId(this.User.Identity.GetUserId()).FirstName,
                PowerPeaks = _unitOfWork.Analysis.GetPeaks(userId, PeakStreamType.Power),
                RunningTime = _unitOfWork.Activity.GetBestTimes(userId),
                CurrentWeight = _unitOfWork.Metrics.GetWeightDetails(userId, 1)[0],
                RecentActivity = _unitOfWork.Activity.GetActivityDto(userId, 7)
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

