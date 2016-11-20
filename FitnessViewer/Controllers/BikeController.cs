using FitnessViewer.Infrastructure.Interfaces;
using FitnessViewer.ViewModels;
using Microsoft.AspNet.Identity;
using System.Web.Mvc;

namespace FitnessViewer.Controllers
{
    [Authorize]
    public class BikeController : Controller
    {
        private IUnitOfWork _unitOfWork;

        public BikeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public ActionResult Index()
        {
            return View();
        }
    }
}