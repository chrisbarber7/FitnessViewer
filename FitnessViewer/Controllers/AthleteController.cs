using FitnessViewer.Infrastructure.enums;
using FitnessViewer.Infrastructure.Helpers;
using FitnessViewer.Infrastructure.Models;
using FitnessViewer.Infrastructure.Models.Dto;
using FitnessViewer.Infrastructure.Repository;
using FitnessViewer.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Web;
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
            var athleteDetails = AthleteDashboardDto.Create(_unitOfWork, userId);

            // invalid user credentials so force a log off 
            if (athleteDetails == null)
            {
                HttpContext.GetOwinContext().Authentication.SignOut();
                return RedirectToAction("Index", "Home");
            }


            return View(athleteDetails);
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

