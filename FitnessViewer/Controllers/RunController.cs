using FitnessViewer.ViewModels;
using Microsoft.AspNet.Identity;
using System.Web.Mvc;

namespace FitnessViewer.Controllers
{
    [Authorize]
    public class RunController : Controller
    {
        private Infrastructure.Data.UnitOfWork _unitOfWork;

        public RunController()
        {
            _unitOfWork = new Infrastructure.Data.UnitOfWork();
        }

        public ActionResult Index()
        {
            return View();
        }
    }
}