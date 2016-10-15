using FitnessViewer.Infrastructure.Repository;
using FitnessViewer.Infrastructure.Models;
using FitnessViewer.ViewModels;
using Microsoft.AspNet.Identity;
using System.Net;
using System.Web.Mvc;
using System.Collections.Generic;
using FitnessViewer.Infrastructure.Models.Dto;
using FitnessViewer.Infrastructure.enums;
using FitnessViewer.Infrastructure.Helpers;
using System;
//using System.Web.Http;

namespace FitnessViewer.Controllers
{
    [Authorize]
    public class ActivityController : Controller
    {
        private Infrastructure.Data.UnitOfWork _unitOfWork;

        public ActivityController()
        {
            _unitOfWork = new Infrastructure.Data.UnitOfWork();
        }

        [Authorize]
        public ActionResult Recalculate(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            StreamHelper.RecalculateSingleActivity(_unitOfWork, id.Value);
            return RedirectToAction("ViewActivity", new { id = id });
        }

        [Authorize]
        public ActionResult ViewActivity(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Activity fvActivity = _unitOfWork.Activity.GetActivity(id.Value);

            if (!fvActivity.DetailsDownloaded)
            {
                ActivityViewModel model = new ActivityViewModel()
                {
                    DetailsDownloaded = false,
                    Name = fvActivity.Name
                };

                return View(model);
            }

            if (fvActivity.Athlete.UserId != User.Identity.GetUserId())
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            return View(ActivityViewModel.CreateFromActivity(_unitOfWork, fvActivity));
        }

        public class SummaryInformationRequest
        {
            public long activityId { get; set; }
            public int startIndex { get; set; }
            public int endIndex { get; set; }
        }

        [HttpGet]
        public ActionResult GetSummaryInformation([System.Web.Http.FromUri] SummaryInformationRequest detail)
        {
            return PartialView("_ActivitySummaryInformation", _unitOfWork.Activity.BuildSummaryInformation(detail.activityId, detail.startIndex, detail.endIndex));
        }

        [Authorize]
        public ActionResult Table()
        {
            return View();
        }

        [Authorize]
        public ActionResult Calendar()
        {
            return View();
        }
    }
}