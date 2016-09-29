using FitnessViewer.Infrastructure.Data;
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
        private Repository _repo;

        public MeasurementController()
        {
            _repo = new Repository();
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

            _repo.AddMeasurement(toAdd);

            return RedirectToAction("Index", "Home");
        }
    }
}