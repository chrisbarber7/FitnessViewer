using System.Web.Mvc;

namespace FitnessViewer.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {

            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Dashboard", "Athlete");

            return View();
        }

    }
}