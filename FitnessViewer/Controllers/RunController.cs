using FitnessViewer.Infrastructure.Data;
using FitnessViewer.Infrastructure.Interfaces;
using FitnessViewer.ViewModels;
using Microsoft.AspNet.Identity;
using System.Web.Mvc;

namespace FitnessViewer.Controllers
{
    [Authorize]
    public class RunController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public RunController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public ActionResult Index()
        {
            return View();
        }
    }
}