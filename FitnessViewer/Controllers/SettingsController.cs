using FitnessViewer.Infrastructure.Interfaces;
using FitnessViewer.Infrastructure.Models;
using FitnessViewer.Infrastructure.Models.ViewModels;
using FitnessViewer.Infrastructure.Repository;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace FitnessViewer.Controllers
{
    [Authorize]
    public class SettingsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
    

        public SettingsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

    public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [Authorize]
        public ActionResult FTPDelete(long? id)
        {
            if (id == null)
                return Json(new { success = false, responseText = "Error Deleting record.", id = id }, JsonRequestBehavior.AllowGet);

            Zone toDelete = _unitOfWork.CRUDRepository.GetByKey<Zone>((int)id.Value);

            // check zone exists and it belongs to the correct user
            if ((toDelete == null) || (toDelete.UserId != User.Identity.GetUserId()))
                return Json(new { success = false, responseText = "Error Deleting record.", id = id }, JsonRequestBehavior.AllowGet);

            _unitOfWork.CRUDRepository.Delete<Zone>(toDelete);
            _unitOfWork.Complete();

            return Json(new { success = true, responseText = "Record Deleted.", id = id }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize]
        public ActionResult UpdateDashboardPeriod(DashboardPeriodViewModel viewModel)
        {
            // update missing data from model 
            viewModel.UserId = User.Identity.GetUserId();
            viewModel.DashboardStart = viewModel.FromDateTime.Value;
            viewModel.DashboardEnd = viewModel.ToDateTime.Value;
   
            if (!ModelState.IsValid)
                return Json(new { success = false, responseText = "Error updating dashboard settings." }, JsonRequestBehavior.AllowGet);

            AthleteSetting settings = _unitOfWork.CRUDRepository.GetByUserId<AthleteSetting>(viewModel.UserId).FirstOrDefault();

            if (settings==null)
                return Json(new { success = false, responseText = "Error updating dashboard settings." }, JsonRequestBehavior.AllowGet);

            try
            {
                settings.DashboardStart = viewModel.DashboardStart;
                settings.DashboardEnd = viewModel.DashboardEnd;
                settings.DashboardRange = viewModel.DashboardRange;

                _unitOfWork.CRUDRepository.Update<AthleteSetting>(settings);
                _unitOfWork.Complete();

                return Json(new { success = true, responseText = "Record Updated Sucessfully." }, JsonRequestBehavior.AllowGet);
            }
            catch(Exception)
            {
                return Json(new { success = false, responseText = "Error updating dashboard settings." }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult FTPUpdate(Zone viewModel)
        {
            // userid not provided so need to populate and clear any model errors.
            viewModel.UserId = User.Identity.GetUserId();
            ModelState["UserId"].Errors.Clear();

            if (!ModelState.IsValid)
                return Json(new { success = false, responseText = "Error.", id = viewModel.Id }, JsonRequestBehavior.AllowGet);

            if (viewModel.Id == int.MaxValue)
            {
                _unitOfWork.CRUDRepository.Add<Zone>(viewModel);
                _unitOfWork.Complete();
                return Json(new { success = true, responseText = "Record Added Sucessfully.", id = viewModel.Id }, JsonRequestBehavior.AllowGet);

            }
            else
            {
                _unitOfWork.CRUDRepository.Update<Zone>(viewModel);
                _unitOfWork.Complete();
                return Json(new { success = true, responseText = "Record Updated Sucessfully.", id = viewModel.Id }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult BikeFTP()
        {
  
            return View(GetPowerZoneInformation());
        }

        public ActionResult BikePower()
        {
            return View();
        }

        public ActionResult BikeHeartRate()
        {
            return View();
        }

        public ActionResult RunHeartRate()
        {
            return View();
        }
        public ActionResult RunPace()
        {
            return View();
        }

        public ActionResult SwimPace()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetZoneGridInformation()
        {
            return PartialView("_zoneGrid", GetPowerZoneInformation());
        }

        private List<Zone> GetPowerZoneInformation()
        {
            var power = _unitOfWork.Settings.GetUserZones(this.User.Identity.GetUserId(), Infrastructure.enums.ZoneType.BikePower).ToList();

            // add an extra row which will be hidden on the grid but enabled/used for editing.
            power.Add(new Infrastructure.Models.Zone()
            {
                Id = int.MaxValue,
                ZoneType = Infrastructure.enums.ZoneType.BikePower,
                StartDate = DateTime.Now,
                Value = 0
            });

            return power.OrderBy(a => a.StartDate).ToList();
        }
    }
}