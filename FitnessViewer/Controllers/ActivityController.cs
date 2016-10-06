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
                SummaryInfo = _unitOfWork.Activity.BuildSummaryInformation(id.Value, 0, int.MaxValue)
        };
            return View(m);
        }

        public class SummaryInformationRequest
        {
            public long activityId { get; set; }
            public int id { get; set; }
            public string type { get; set; }
        }

        [HttpGet]
        public ActionResult GetSummaryInformation([System.Web.Http.FromUri] SummaryInformationRequest detail)
        {
            int startIndex = 0;
            int endIndex = 0;

            // depending on whether a lap or power/heartrate/cadence was selected start/end index will be found in a different table.
            GetStartAndEndIndex(detail, ref startIndex, ref endIndex);

            return PartialView("_ActivitySummaryInformation", _unitOfWork.Activity.BuildSummaryInformation(detail.activityId, startIndex, endIndex));
        }

        /// <summary>
        /// Lookup start index and end index in the stream depending on whether a lap or power/heart rate/cadence was selected.
        /// </summary>
        /// <param name="detail"></param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        private void GetStartAndEndIndex(SummaryInformationRequest detail, ref int startIndex, ref int endIndex)
        {
            if (detail.type.ToUpper() == "LAP")
            {
                Lap l = _unitOfWork.Activity.GetLap(detail.id);
                if (l != null)
                {
                    startIndex = l.StartIndex;
                    endIndex = l.EndIndex;
                }
            }
            else
            {
                ActivityPeakDetail p = _unitOfWork.Activity.GetActivityPeakDetail(detail.id);
                if (p != null)
                {
                    if (p.StartIndex != null)
                    {
                        startIndex = p.StartIndex.Value;
                        endIndex = p.StartIndex.Value + p.Duration;
                    }
                }
            }
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