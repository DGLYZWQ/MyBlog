using System.Web.Mvc;
using BlogSystem.MVCSite.Filter;

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