using FitnessViewer.Infrastructure.Models;
using FitnessViewer.ViewModels;
using Microsoft.AspNet.Identity;
using System.Web.Mvc;

namespace FitnessViewer.Controllers
{
    [Authorize]
    public class MetricsController : Controller
    {
        private Infrastructure.Data.UnitOfWork _unitOfWork;

        public MetricsController()
        {
            _unitOfWork = new Infrastructure.Data.UnitOfWork();
        }

        public ActionResult Index()
        {
            return View();
        }


        [Authorize]
        public ActionResult AddWeight()
        {
            return View(new MeasurementViewModel());
        }

        [HttpPost]
        [Authorize]
        public ActionResult AddWeight(MeasurementViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View("AddWeight", viewModel);

            Metric w = Metric.CreateMetric(User.Identity.GetUserId(), Infrastructure.enums.MetricType.Weight, viewModel.GetRecordedDateTime(), viewModel.Weight, true); _unitOfWork.Metrics.AddMetric(w);

            if (viewModel.Bodyfat != null)
            {
                Metric bf = Metric.CreateMetric(User.Identity.GetUserId(), Infrastructure.enums.MetricType.BodyFat, viewModel.GetRecordedDateTime(), viewModel.Bodyfat.Value, true);
                _unitOfWork.Metrics.AddMetric(bf);
            }

            _unitOfWork.Complete();
            return RedirectToAction("Index", "Home");
        }
    }
}




