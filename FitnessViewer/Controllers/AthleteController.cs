using FitnessViewer.Infrastructure.Data;
using FitnessViewer.ViewModels;
using Microsoft.AspNet.Identity;
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

            var result = new AthleteViewModel()
            {
                FirstName = _repo.FindAthleteByUserId(this.User.Identity.GetUserId()).FirstName,
                PowerPeaks = _repo.GetPeaks(userId, Infrastructure.Helpers.PeakStreamType.Power),
                RunningTime = _repo.GetBestTimes(userId)
            };

            return View(result);
        }
    }
}

