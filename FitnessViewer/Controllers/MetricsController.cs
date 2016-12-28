using FitnessViewer.Infrastructure.Data;
using FitnessViewer.Infrastructure.enums;
using FitnessViewer.Infrastructure.Helpers;
using FitnessViewer.Infrastructure.Interfaces;
using FitnessViewer.Infrastructure.Models;
using FitnessViewer.Infrastructure.Models.Dto;
using FitnessViewer.ViewModels;
using Microsoft.AspNet.Identity;
using System.Web.Mvc;

namespace FitnessViewer.Controllers
{
    [Authorize]
    public class MetricsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public MetricsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public ActionResult Index()
        {

            var userId = this.User.Identity.GetUserId();
            var dashboard =  AthleteDto.Create(_unitOfWork, userId);
            return View(dashboard);
        }


        [Authorize]
        public ActionResult Weight()
        {
            return View(new WeightViewModel());
        }

        [HttpPost]
        [Authorize]
        public ActionResult AddWeight(WeightViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View("Weight", viewModel);

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

            if (viewModel.MoveOntoNextDay)
            {
                WeightViewModel vm = new WeightViewModel(viewModel.GetRecordedDateTime().AddDays(1));
                vm.MoveOntoNextDay = true;
                return RedirectToAction("Weight", vm);
            }

            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public ActionResult HeartRate(HeartRateViewModel vm)
        {
            if (vm == null)
                vm = new HeartRateViewModel();

            return View(vm);
       
        }
        [HttpPost]
        [Authorize]
        public ActionResult AddHeartRate(HeartRateViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View("HeartRate", viewModel);

            Metric heartRate = Metric.CreateMetric(User.Identity.GetUserId(), Infrastructure.enums.MetricType.RestingHeartRate, viewModel.GetRecordedDateTime(), viewModel.RestingHeartRate, true);
            _unitOfWork.Metrics.AddOrUpdateMetric(heartRate);

            _unitOfWork.Complete();

            if (viewModel.MoveOntoNextDay)
            {
                HeartRateViewModel vm = new HeartRateViewModel(viewModel.GetRecordedDateTime().AddDays(1));
                vm.MoveOntoNextDay = true;
                return RedirectToAction("HeartRate", vm);
            }

            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public ActionResult HRV()
        {
            return View(new HRVViewModel());
        }

        [HttpPost]
        [Authorize]
        public ActionResult AddHRV(HRVViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View("HRV", viewModel);

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

            if (viewModel.MoveOntoNextDay)
            {
                HRVViewModel vm = new HRVViewModel(viewModel.GetRecordedDateTime().AddDays(1));
                vm.MoveOntoNextDay = true;
                return RedirectToAction("HRV", vm);
            }
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public ActionResult Sleep(SleepViewModel vm)
        {
            if (vm == null)
                vm = new SleepViewModel();

            return View(vm);

        }
        [HttpPost]
        [Authorize]
        public ActionResult AddSleep(SleepViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View("Sleep", viewModel);

            Metric sleep = Metric.CreateMetric(User.Identity.GetUserId(), Infrastructure.enums.MetricType.SleepMinutes, viewModel.GetRecordedDateTime(), viewModel.GetSleepMinutes(), true);
            _unitOfWork.Metrics.AddOrUpdateMetric(sleep);

            _unitOfWork.Complete();

            if (viewModel.MoveOntoNextDay)
            {
                SleepViewModel vm = new SleepViewModel(viewModel.GetRecordedDateTime().AddDays(1));
                vm.MoveOntoNextDay = true;
                return RedirectToAction("Sleep", vm);
            }

            return RedirectToAction("Index", "Home");
        }
    }
}




