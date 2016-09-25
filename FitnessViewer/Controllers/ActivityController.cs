using FitnessViewer.Infrastructure.Data;
using FitnessViewer.Infrastructure.Models;
using FitnessViewer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FitnessViewer.Controllers
{
    [Authorize]
    public class ActivityController : Controller
    {
        private Repository _repo;

        public ActivityController()
        {

            _repo = new Repository();
        }
        public ActionResult View(long id)
        {
            Activity a = _repo.GetActivity(id);

            ActivityViewModel m = new ActivityViewModel()
            {
                Id = a.Id,
                Name = a.Name,
                Distance = a.Distance
            };
            return View(m);
        }
    }
}