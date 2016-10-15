using FitnessViewer.ViewModels;
using Microsoft.AspNet.Identity;
using System.Web.Mvc;

namespace FitnessViewer.Controllers
{
    [Authorize]
    public class SwimController : Controller
    {
        private Infrastructure.Data.UnitOfWork _unitOfWork;

        public SwimController()
        {
            _unitOfWork = new Infrastructure.Data.UnitOfWork();
        }

        public ActionResult Index()
        {
            return View();
        }
    }
}