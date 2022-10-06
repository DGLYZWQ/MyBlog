using System.Web.Mvc;
using BlogSystem.IBLL;

namespace BlogSystem.MVCSite.Areas.Backend.Controllers
{
    public class RolesBackendController : Controller
    {
        // GET : Backend/RolesBackend
        private IRolesBll _rolesBll;

        public RolesBackendController(IRolesBll rolesBll)
        {
            _rolesBll = rolesBll;
        }
        public ActionResult List()
        {
            return View();
        }
    }
}