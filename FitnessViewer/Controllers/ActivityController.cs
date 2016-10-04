﻿using FitnessViewer.Infrastructure.Repository;
using FitnessViewer.Infrastructure.Models;
using FitnessViewer.ViewModels;
using Microsoft.AspNet.Identity;
using System.Net;
using System.Web.Mvc;
using System.Collections.Generic;
using FitnessViewer.Infrastructure.Models.Dto;
using FitnessViewer.Infrastructure.enums;
using FitnessViewer.Infrastructure.Helpers;

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

            IEnumerable<ActivityLap> laps = _unitOfWork.Activity.GetLaps(id.Value);
            IEnumerable<ActivityLap> power = _unitOfWork.Activity.GetLapStream(id.Value, PeakStreamType.Power);
            IEnumerable<ActivityLap> heartRate = _unitOfWork.Activity.GetLapStream(id.Value, PeakStreamType.HeartRate);
            IEnumerable<ActivityLap> cadence = _unitOfWork.Activity.GetLapStream(id.Value, PeakStreamType.Cadence);
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
                Laps = laps,
                Power = power,
                HeartRate = heartRate,
                Cadence=cadence
                
            };
            return View(m);
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