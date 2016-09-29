using FitnessViewer.Infrastructure.Repository;
using FitnessViewer.Infrastructure.Models;
using FitnessViewer.ViewModels;
using Microsoft.AspNet.Identity;
using System.Net;
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

        [Authorize]
        public ActionResult View(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Activity a = _repo.GetActivity(id.Value);

            if (a.Athlete.UserId != User.Identity.GetUserId())
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ActivityViewModel m = new ActivityViewModel()
            {
                Id = a.Id,
                Name = a.Name,
                Distance = a.GetDistanceByActivityType()
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