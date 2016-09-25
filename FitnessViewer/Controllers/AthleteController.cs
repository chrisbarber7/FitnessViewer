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
            Athlete data = _repo.FindAthleteByUserId(this.User.Identity.GetUserId());
            IEnumerable<ActivityViewModel> activites = Mapper.Map<IEnumerable<ActivityViewModel>>( _repo.GetActivities(this.User.Identity.GetUserId()));
            var result = new AthleteViewModel() { FirstName = data.FirstName, Activities=activites };

            return View(result);
        }
    }
}

