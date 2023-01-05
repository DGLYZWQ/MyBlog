using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using BlogSystem.BLL;
using BlogSystem.IBLL;
using BlogSystem.MVCSite.Areas.Backend.Data.Login;
using System.Linq;
namespace BlogSystem.MVCSite.Areas.Backend.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUsersBll _usersSvc;
        private readonly IRolesBll _rolesBll;

        public LoginController(IUsersBll usersSvc,IRolesBll rolesBll)
        {
            _usersSvc = usersSvc;
            _rolesBll = rolesBll;
        }


        [HttpGet]
        public ActionResult SignUp()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        public async Task<ActionResult> SignUp(RegisterViewModel model)
        {

            if (ModelState.IsValid)
            {
                var rolesList =await _rolesBll.GetRolesListByPageAsync(1, 1, "管理员", true);
                model.RolesId = rolesList.First().Id;
                var info =await _usersSvc.GetUsersByEmail(model.Email);
                if(info!=null)
                {
                    ModelState.AddModelError("Email", "电子邮箱已存在");
                    return View(model);
                }
                await _usersSvc.RegisterAsync(model.Email, Models.MD5Helper.GetMD5String(model.Password) ,model.RolesId);
                return Content("<script>alert('注册成功');location.href='/Backend/Login/SignIn'</script>");    
                //return RedirectToAction("SignIn", "Login");
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
            var entity = new LoginViewModel();
            if (Request.Cookies["Email"]!=null&&Request.Cookies["Password"]!=null)
            {
                entity.Email = Request.Cookies["Email"].Value;
                entity.Password = Request.Cookies["Password"].Value;
            }
            return View(entity);
        }

        [HttpPost]
        public async Task<ActionResult> SignIn(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var data = await _usersSvc.LoginAsync(model.Email, Models.MD5Helper.GetMD5String(model.Password));
                if (data != null)
                {
                    var rolesList = await _rolesBll.GetRolesListByPageAsync(1, 1, "管理员", true);
                    var RolesId = rolesList.First().Id;
                    if (data.RolesId != RolesId)
                    {
                        ModelState.AddModelError("Password", "用户名或者密码错误");
                        return View(model);
                    }
                    //帐号密码正确，需要判断是否记住这个帐号密码
                    if (model.RememberMe)
                    {
                        HttpCookie u_cookie = new HttpCookie("Email",data.Email);
                        HttpCookie r_cookie = new HttpCookie("Password", model.Password);
                        u_cookie.Expires = DateTime.Now.AddDays(7);
                        r_cookie.Expires = DateTime.Now.AddDays(7);
                        Response.Cookies.Add(u_cookie);
                        Response.Cookies.Add(r_cookie);
                    }
                    else
                    {
                        if (Request.Cookies["Email"] != null && Request.Cookies["Password"] != null)
                        {
                            var cookie1 = Request.Cookies["Email"];
                            cookie1.Expires = DateTime.Now.AddMinutes(-1);
                            Response.Cookies.Add(cookie1);
                            var cookie2 = Request.Cookies["Password"];
                            cookie2.Expires = DateTime.Now.AddMinutes(-1);
                            Response.Cookies.Add(cookie2);
                        }
                    }
                    Session["LoginOK"] = data.Email;
                    Session["RolesId"] = data.RolesId;
                    Session["admin"] = data;

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("Password", "用户名或者密码错误");
                    return View(model);
                }
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult ForgetPwd()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        public async Task<ActionResult> ForgetPwd(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var data = await _usersSvc.ResetPwd(model.Email, Models.MD5Helper.GetMD5String(model.Password));
                if (data > 0) {


                    return Content("<script>alert('密码重置成功');location.href='/Backend/Login/SignIn'</script>");
                }
            }

            return View(model);
        }
    }
}