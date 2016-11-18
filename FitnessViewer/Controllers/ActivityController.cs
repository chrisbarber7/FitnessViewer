using FitnessViewer.Infrastructure.Repository;
using FitnessViewer.Infrastructure.Models;
using FitnessViewer.Infrastructure.Models.ViewModels;
using Microsoft.AspNet.Identity;
using System.Net;
using System.Web.Mvc;
using System.Collections.Generic;
using FitnessViewer.Infrastructure.Models.Dto;
using FitnessViewer.Infrastructure.enums;
using FitnessViewer.Infrastructure.Helpers;
using System;
using FitnessViewer.Infrastructure.Models.Collections;

using System.Threading.Tasks;
using AutoMapper;

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

            ActivityDetailDto v = ActivityDetailDto.CreateFromActivity(_unitOfWork, fvActivity);
            v.SummaryInfo.Label = "Activity";
            return View(v);
        }

        public class SummaryInformationRequest
        {
            public long activityId { get; set; }
            public string selection { get; set; }
            public int startIndex { get; set; }
            public int endIndex { get; set; }

        }

        [HttpGet]
        public ActionResult GetSummaryInformation([System.Web.Http.FromUri] SummaryInformationRequest detail)
        {
            ActivityStreams details = ActivityStreams.CreateFromExistingActivityStream(detail.activityId, detail.startIndex, detail.endIndex);

            ActivityMinMaxDto mma = new ActivityMinMaxDto(details);
            mma.Populate();
            mma.Label = detail.selection;

            return PartialView("_ActivitySummaryInformation", mma);
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

        //// GET: People/Edit/{id}
        //public async Task<ActionResult> Edit(int? id)
        public ActionResult Edit(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Activity a = _unitOfWork.Activity.GetActivity(id.Value);

            if ((a == null) || (a.Athlete.UserId != User.Identity.GetUserId()))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            return PartialView("_Edit", Mapper.Map<EditActivityViewModel>(a));
        }

        // POST: Activity/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EditActivityViewModel activity)
        {
            if (ModelState.IsValid)
            {
                Activity a = _unitOfWork.Activity.GetActivity(activity.Id);

                if ((a == null) || (a.Athlete.UserId != User.Identity.GetUserId()))
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

   
                StravaUpdate upd = new StravaUpdate(_unitOfWork, User.Identity.GetUserId());
             await    upd.ActivityDetailsUpdate(a, activity);
           
              

                return Json(new { success = true });
            }
            return PartialView("_Edit", activity);
        }
    }
}