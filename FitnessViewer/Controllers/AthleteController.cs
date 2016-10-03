using FitnessViewer.Infrastructure.enums;
using FitnessViewer.Infrastructure.Repository;
using FitnessViewer.ViewModels;
using Microsoft.AspNet.Identity;
using System.Web.Mvc;

namespace FitnessViewer.Controllers
{
    [Authorize]
    public class AthleteController : Controller
    {
        private Infrastructure.Data.UnitOfWork _unitOfWork;

        public AthleteController()
        {

            _unitOfWork = new Infrastructure.Data.UnitOfWork();
        }

        public ActionResult Dashboard()
        {
            var userId = this.User.Identity.GetUserId();

            var result = new AthleteViewModel()
            {
                FirstName = _unitOfWork.Athlete.FindAthleteByUserId(this.User.Identity.GetUserId()).FirstName,
                PowerPeaks = _unitOfWork.Analysis.GetPeaks(userId, PeakStreamType.Power),
                RunningTime = _unitOfWork.Activity.GetBestTimes(userId)
            };

            return View(result);
        }

        public ActionResult ActivityScan()
        {
            _unitOfWork.Queue.AddQueueItem(this.User.Identity.GetUserId());
            _unitOfWork.Complete();
            return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri);
        }
    }
}

