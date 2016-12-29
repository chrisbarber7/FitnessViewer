using FitnessViewer.Infrastructure.Interfaces;
using FitnessViewer.Infrastructure.Models.Dto;
using FitnessViewer.ViewModels;
using Microsoft.AspNet.Identity;
using System.Web.Mvc;

namespace FitnessViewer.Controllers
{
    [Authorize]
    public class BikeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public BikeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public ActionResult BikeIndex()
        {
            var userId = this.User.Identity.GetUserId();
            var dashboard = AthleteDto.Create(_unitOfWork, userId);
            return View(dashboard);
        }
    }
}