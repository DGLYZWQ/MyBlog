using System;
using System.Web.Mvc;

namespace BlogSystem.MVCSite.Controllers
{
    public class FrontHomeController : Controller
    {



        public ActionResult Login()
        {
            return View();
        }




        // GET
        public ActionResult Index()
        {
            return View();
        }
    }
}