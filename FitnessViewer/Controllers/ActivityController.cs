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

            StreamHelper.RecalculateSingleActivity(id.Value);



            return RedirectToAction("ViewActivity", new { id = id });
        }


        [Authorize]
        public ActionResult ViewActivity(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Activity a = _unitOfWork.Activity.GetActivity(id.Value);

            if (a.Athlete.UserId != User.Identity.GetUserId())
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);


            ActivitySummaryInformation summ = new ActivitySummaryInformation() { Distance = "100", Time = "08:01:02" };

            ActivityViewModel m = new ActivityViewModel()
            {
                Id = a.Id,
                Name = a.Name,
                Distance = a.GetDistanceByActivityType(),
                AverageSpeed = 0,
                ElevationGain = a.ElevationGain,
                ActivityTypeId = a.ActivityTypeId,
                Date = a.StartDateLocal.ToShortDateString(),
                ElapsedTime = a.ElapsedTime.Value,
                Laps = _unitOfWork.Activity.GetLaps(id.Value),
                Power = _unitOfWork.Activity.GetLapStream(id.Value, PeakStreamType.Power),
                HeartRate = _unitOfWork.Activity.GetLapStream(id.Value, PeakStreamType.HeartRate),
                Cadence = _unitOfWork.Activity.GetLapStream(id.Value, PeakStreamType.Cadence),
                SummaryInfo = summ
                
                
            };
            return View(m);
        }

        public class SummaryInformationRequest
        {
            public int id { get; set; }
            public int type { get; set; }
        }

        [HttpGet]
        public ActionResult GetSummaryInformation([System.Web.Http.FromUri] SummaryInformationRequest detail)
        {
            ActivitySummaryInformation s = new ActivitySummaryInformation() { Distance = detail.id.ToString(), Time = DateTime.Now.ToShortTimeString() };
            return PartialView("_ActivitySummaryInformation", s);

         //   return Ok(s);
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