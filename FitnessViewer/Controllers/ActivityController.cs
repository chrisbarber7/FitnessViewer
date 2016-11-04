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
using FitnessViewer.Infrastructure.Models.Collections;


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

            ActivityStreams.CreateFromExistingActivityStream(id.Value)
                .CalculatePeaksAndSave();

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
                ActivityDetailDto model = new ActivityDetailDto()
                {
                    DetailsDownloaded = false,
                    Name = fvActivity.Name
                };

                return View(model);
            }

            if (fvActivity.Athlete.UserId != User.Identity.GetUserId())
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            
            return View(ActivityDetailDto.CreateFromActivity(_unitOfWork, fvActivity));
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

            var details = ActivityStreams.CreateFromExistingActivityStream(detail.activityId, detail.startIndex, detail.endIndex).BuildSummaryInformation();
            return PartialView("_ActivitySummaryInformation", details);
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