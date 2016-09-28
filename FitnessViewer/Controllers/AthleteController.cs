using AutoMapper;
using FitnessViewer.Infrastructure.Data;
using FitnessViewer.Infrastructure.Models;
using FitnessViewer.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace FitnessViewer.Controllers
{

    [Authorize]
    public class AthleteController : Controller
    {
        private Repository _repo;

        public AthleteController()
        {

            _repo = new Repository();
        }

        public ActionResult Dashboard()
        {
            var userId = this.User.Identity.GetUserId();
            var powerPeakInfo = _repo.GetPeaks(userId, Infrastructure.Helpers.PeakStreamType.Power);
            var runningBestTimes = _repo.GetBestTimes(userId);

            Athlete data = _repo.FindAthleteByUserId(this.User.Identity.GetUserId());
            var result = new AthleteViewModel()
            {
                FirstName = data.FirstName,
                PowerPeaks = powerPeakInfo,
                RunningTime = runningBestTimes
            };

            int[] test  = { 1, 5, 3, 7, 9,5,7 };

            ViewBag.intArray = test;


            return View(result);
        }
    }
}

