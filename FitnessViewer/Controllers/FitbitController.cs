﻿using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Fitbit.Api;
using System.Configuration;
using Fitbit.Models;
using Fitbit.Api.Portable;
using System.Threading.Tasks;
using Fitbit.Api.Portable.OAuth2;
using FitnessViewer.Infrastructure.Models;
using FitnessViewer.Infrastructure.Helpers;
using Microsoft.AspNet.Identity;
using FitnessViewer.Infrastructure.Data;
using FitnessViewer.Infrastructure.Models.Dto;

namespace SampleWebMVC.Controllers
{
    public class FitbitController : Controller
    {
        private UnitOfWork _unitOfWork;

        public FitbitController()
        {
            _unitOfWork = new UnitOfWork();
        }

        public ActionResult Index()
        {
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Home()
        {
            return View(PopulateModel());
        }

        private FitbitHomeDto PopulateModel()
        {
            FitbitUser user = _unitOfWork.Metrics.GetFitbitUser(User.Identity.GetUserId());

            FitbitHomeDto model = new FitbitHomeDto();
            model.Authorised = user != null ? true : false;

            return model;
        }

        /// <summary>
        /// Part 1 of authorisation.  Pass user to fitbit to authorise app (fitbit client id/secret required which are stored in web.config)
        /// </summary>
        /// <returns></returns>
        public ActionResult Authorize()
        {
            var authenticator = new OAuth2Helper(FitbitHelper.GetFitbitAppCredentials(), Request.Url.GetLeftPart(UriPartial.Authority) + "/Fitbit/Callback");
            string[] scopes = new string[] { "profile","heartrate", "nutrition", "sleep", "weight" };

            string authUrl = authenticator.GenerateAuthUrl(scopes, null);

            return Redirect(authUrl);
        }

        //Final step. Take this authorization information and use it in the app
        /// <summary>
        /// Part 2 of authorisation.  User has (hopefully) accepted request and fitbit passed authorisation code.
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Callback()
        {
            FitbitAppCredentials appCredentials = FitbitHelper.GetFitbitAppCredentials();
            var authenticator = new OAuth2Helper(appCredentials, Request.Url.GetLeftPart(UriPartial.Authority) + "/Fitbit/Callback");
            string code = Request.Params["code"];

            // ask for access token.
            OAuth2AccessToken accessToken = await authenticator.ExchangeAuthCodeForAccessTokenAsync(code);

            // save token (user may have previously authorised in which case update)
            FitbitHelper.AddOrUpdateUser(_unitOfWork, User.Identity.GetUserId(), accessToken);

            return View("Home", PopulateModel());
        }
  
        /// <summary>
        /// Test method for downloading data from fitbit.
        /// </summary>
        /// <returns></returns>
        public ActionResult Download()
        {
            DownloadQueue job = DownloadQueue.CreateQueueJob(User.Identity.GetUserId(), FitnessViewer.Infrastructure.enums.DownloadType.Fitbit);
            _unitOfWork.Queue.AddQueueItem(job);
            _unitOfWork.Complete();
            AzureWebJob.AddToAzureQueue(job.Id);
            
            // trigger web job to download activity details.
            AzureWebJob.CreateTrigger(_unitOfWork);

            return View();
        }
    }
}