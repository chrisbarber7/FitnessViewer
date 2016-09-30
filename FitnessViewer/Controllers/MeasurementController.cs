using FitnessViewer.Infrastructure.Repository;
using FitnessViewer.Infrastructure.Models;
using FitnessViewer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace FitnessViewer.Controllers
{
    public class MeasurementController : Controller
    {
        private Infrastructure.Data.UnitOfWork _unitOfWork;

        public MeasurementController()
        {
            _unitOfWork = new Infrastructure.Data.UnitOfWork();
        }

        public object Measurement { get; private set; }

        [Authorize]
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult Add(MeasurementViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View("Add", viewModel);

            Measurement toAdd = new Measurement
            {
                UserId = User.Identity.GetUserId(),
                Recorded = viewModel.GetRecordedDateTime(),
                Weight = viewModel.Weight,
                Bodyfat = viewModel.Bodyfat
            };

            _unitOfWork.Metrics.AddMeasurement(toAdd);
            _unitOfWork.Complete();
            return RedirectToAction("Index", "Home");
        }
    }
}