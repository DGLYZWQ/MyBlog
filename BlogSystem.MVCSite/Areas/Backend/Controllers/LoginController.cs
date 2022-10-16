using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using BlogSystem.BLL;
using BlogSystem.IBLL;
using BlogSystem.MVCSite.Areas.Backend.Data.Login;

namespace BlogSystem.MVCSite.Areas.Backend.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUsersBll _usersSvc;

        public LoginController(IUsersBll usersSvc)
        {
            _usersSvc = usersSvc;
        }


        [HttpGet]
        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> SignUp(RegisterViewModel model)
        {

            if (!ModelState.IsValid)
            {
                await _usersSvc.RegisterAsync(model.Email, model.Password ,model.RolesId);
                //return Content("<script>alert('注册成功');location.href='../../../Backend/Login/SignIn'</script>");    
                return RedirectToAction("SignIn", "Login");
            }
            return View(model);
        }

        //public ActionResult Register(RegisterViewModel admin)
        //{
        //    if (admin != null)
        //    {
        //        db.Entry(admin).State = System.Data.Entity.EntityState.Added;
        //        db.RegisterViewModel.Add(admin);
        //    }

        //    db.Users.Add(admin);
        //    return View();
        //}

            
        


        [HttpGet]
        public ActionResult SignIn()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        public async Task<ActionResult> SignIn(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var data = await _usersSvc.LoginAsync(model.Email, model.Password);
                if (data != null)
                {
                    //帐号密码正确，需要判断是否记住这个帐号密码
                    if (model.RememberMe)
                    {
                        HttpCookie u_cookie = new HttpCookie("LoginOK",data.Email);
                        HttpCookie r_cookie = new HttpCookie("RolesId", data.RolesId.ToString());
                        u_cookie.Expires = DateTime.Now.AddDays(7);
                        r_cookie.Expires = DateTime.Now.AddDays(7);
                        Response.Cookies.Add(u_cookie);
                        Response.Cookies.Add(r_cookie);
                    }
                    else
                    {
                        Session["LoginOK"] = data.Email;
                        Session["RolesId"] = data.RolesId;
                    }

                    return RedirectToAction("Index", "Home");
                }
            }

            return View(model);
        }
    }
}