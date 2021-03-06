﻿using FitnessViewer.Infrastructure.enums;
using FitnessViewer.Infrastructure.Helpers;
using FitnessViewer.Infrastructure.Helpers.Conversions;
using FitnessViewer.Infrastructure.Interfaces;
using FitnessViewer.Infrastructure.Models;
using FitnessViewer.Infrastructure.Models.Dto;
using FitnessViewer.Infrastructure.Repository;
using FitnessViewer.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Net;
using System.Web;
using System.Web.Mvc;
using static FitnessViewer.Infrastructure.Helpers.DateHelpers;

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

        public class SportSummaryRequestInformation : DateRange
        {
            public string Sport { get; set; }
        }

        [System.Web.Http.HttpGet]
        public ActionResult GetSportSummary([System.Web.Http.FromUri] SportSummaryRequestInformation detail)
        {
            SportType sportType;
            try
            {
                sportType = EnumConversion.GetEnumFromDescription<SportType>(detail.Sport);
            }
            catch(ArgumentException)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SportSummaryDto details = DashboardSportSummary.Create(this.User.Identity.GetUserId(),sportType, detail.FromDateTime.Value, detail.ToDateTime.Value);

         
            return PartialView("_sportSummary", details);
        }

        public ActionResult Bike()
        {
            var userId = this.User.Identity.GetUserId();
            var dashboard = AthleteDto.Create(_unitOfWork, userId);
            return View(dashboard);
        }
        
        public ActionResult Metrics()
        {
            var userId = this.User.Identity.GetUserId();
            var dashboard = AthleteDto.Create(_unitOfWork, userId);
            return View(dashboard);
        }

        public ActionResult Swim()
        {
            var userId = this.User.Identity.GetUserId();
            var dashboard = AthleteDto.Create(_unitOfWork, userId);
            return View(dashboard);
        }
        public ActionResult Run()
        {
            var userId = this.User.Identity.GetUserId();
            var dashboard = AthleteDto.Create(_unitOfWork, userId);
            return View(dashboard);
        }
    }
}

