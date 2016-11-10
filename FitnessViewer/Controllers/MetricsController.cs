using FitnessViewer.Infrastructure.enums;
using FitnessViewer.Infrastructure.Helpers;
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
            return View(new WeightViewModel());
        }

        [HttpPost]
        [Authorize]
        public ActionResult AddWeight(WeightViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View("AddWeight", viewModel);

            Metric w = Metric.CreateMetric(User.Identity.GetUserId(), Infrastructure.enums.MetricType.Weight, viewModel.GetRecordedDateTime(), viewModel.Weight, true); _unitOfWork.Metrics.AddOrUpdateMetric(w);
            _unitOfWork.Metrics.AddOrUpdateMetric(w);

            if (viewModel.Bodyfat != null)
            {
                Metric bf = Metric.CreateMetric(User.Identity.GetUserId(), Infrastructure.enums.MetricType.BodyFat, viewModel.GetRecordedDateTime(), viewModel.Bodyfat.Value, true);
                _unitOfWork.Metrics.AddOrUpdateMetric(bf);
            }

            _unitOfWork.Complete();

            // as weight details have changed we need to refresh weights recorded against activities
            ActivityWeight aw = new ActivityWeight(User.Identity.GetUserId());
            aw.UpdateActivityWeight();

            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public ActionResult AddHeartRate()
        {
            return View(new HeartRateViewModel());
        }
        [HttpPost]
        [Authorize]
        public ActionResult AddHeartRate(HeartRateViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View("AddHeartRate", viewModel);

            Metric heartRate = Metric.CreateMetric(User.Identity.GetUserId(), Infrastructure.enums.MetricType.RestingHeartRate, viewModel.GetRecordedDateTime(), viewModel.RestingHeartRate, true);
            _unitOfWork.Metrics.AddOrUpdateMetric(heartRate);

            _unitOfWork.Complete();
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public ActionResult AddHRV()
        {
            return View(new HRVViewModel());
        }

        [HttpPost]
        [Authorize]
        public ActionResult AddHRV(HRVViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View("AddHRV", viewModel);

            Metric m1 = Metric.CreateMetric(User.Identity.GetUserId(), MetricType.HRV, viewModel.GetRecordedDateTime(), viewModel.HRV.Value, true);
            _unitOfWork.Metrics.AddOrUpdateMetric(m1);

            if (viewModel.HRVReadiness.HasValue)
            {
                Metric m2 = Metric.CreateMetric(User.Identity.GetUserId(), MetricType.HRVReadiness, viewModel.GetRecordedDateTime(), viewModel.HRVReadiness.Value, true);
                _unitOfWork.Metrics.AddOrUpdateMetric(m2);
            }
            if (viewModel.HRVRMSSD.HasValue)
            {
                Metric m3 = Metric.CreateMetric(User.Identity.GetUserId(), MetricType.HRVRMSSD, viewModel.GetRecordedDateTime(), viewModel.HRVRMSSD.Value, true);
                _unitOfWork.Metrics.AddOrUpdateMetric(m3);
            }
            if (viewModel.HRVLnRMSSD.HasValue)
            {
                Metric m4 = Metric.CreateMetric(User.Identity.GetUserId(), MetricType.HRVLnRMSSD, viewModel.GetRecordedDateTime(), viewModel.HRVLnRMSSD.Value, true);
                _unitOfWork.Metrics.AddOrUpdateMetric(m4);
            }
            if (viewModel.HRVSDNN.HasValue)
            {
                Metric m5 = Metric.CreateMetric(User.Identity.GetUserId(), MetricType.HRVSDNN, viewModel.GetRecordedDateTime(), viewModel.HRVSDNN.Value, true);
                _unitOfWork.Metrics.AddOrUpdateMetric(m5);
            }
            if (viewModel.HRVNN50.HasValue)
            {
                Metric m6 = Metric.CreateMetric(User.Identity.GetUserId(), MetricType.HRVNN50, viewModel.GetRecordedDateTime(), viewModel.HRVNN50.Value, true);
                _unitOfWork.Metrics.AddOrUpdateMetric(m6);
            }
            if (viewModel.HRVPNN50.HasValue)
            {
                Metric m7 = Metric.CreateMetric(User.Identity.GetUserId(), MetricType.HRVPNN50, viewModel.GetRecordedDateTime(), viewModel.HRVPNN50.Value, true);
                _unitOfWork.Metrics.AddOrUpdateMetric(m7);
            }

            _unitOfWork.Complete();
            return RedirectToAction("Index", "Home");
        }

    }

}




