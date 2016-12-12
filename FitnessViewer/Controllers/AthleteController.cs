using FitnessViewer.Infrastructure.enums;
using FitnessViewer.Infrastructure.Helpers;
using FitnessViewer.Infrastructure.Interfaces;
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
        private readonly IUnitOfWork _unitOfWork;

        public AthleteController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public ActionResult Dashboard()
        {
            var userId = this.User.Identity.GetUserId();
            var dashboard = new AthleteDashboardDto(_unitOfWork, userId);
            
            // invalid user credentials so force a log off 
            if (!dashboard.Populate())
            {
                HttpContext.GetOwinContext().Authentication.SignOut();
                return RedirectToAction("Index", "Home");
            }

            return View(dashboard);
        }

        public ActionResult ActivityScan()
        {
            DownloadQueue.CreateQueueJob(this.User.Identity.GetUserId(), DownloadType.Strava).Save();
            return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri);
        }

        public ActionResult ReprocessJobs()
        {
            foreach(DownloadQueue queueJob in _unitOfWork.Queue.GetFailedJob())
                new ProcessQueueJob(queueJob.Id).ResumbitJob();
       
            return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri);
        }
    }
}

