using System.Web.Mvc;

namespace BlogSystem.MVCSite.Areas.Backend.Controllers
{
    public class HomeController : Controller
    {
        // GET
        public ActionResult Index()
        {
            return View();
        }
    }
}