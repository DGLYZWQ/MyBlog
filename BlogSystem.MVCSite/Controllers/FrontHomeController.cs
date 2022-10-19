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


        public ActionResult Register()
        {
            return View();
        }

        public ActionResult ForgetPassword()
        {
            return View();
        }

        public ActionResult ChangePassword()
        {
            return View();
        }


        // GET
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AboutUs()
        {
            return View();
        }

        public ActionResult ContactUs()
        {
            return View();
        }
    }
}