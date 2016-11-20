using FitnessViewer.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FitnessViewer.Controllers
{
    [Authorize]
    public class SettingsController : Controller
    {
        private IUnitOfWork _unitOfWork;
        public SettingsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

    public ActionResult Index()
        {
            return View();
        }

        public ActionResult Power()
        {
            return View();
        }

        public ActionResult Pace()
        {
            return View();
        }

        public ActionResult HeartRate(string sport)
        {
            return View();
        }
    }
}