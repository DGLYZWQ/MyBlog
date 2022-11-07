using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using BlogSystem.Dtos;
using BlogSystem.IBLL;
using BlogSystem.MVCSite.Areas.Backend.Data.Layout;
using BlogSystem.MVCSite.Filter;

namespace BlogSystem.MVCSite.Areas.Backend.Controllers
{
    
    public class LayoutController : Controller
    {
        private IUsersBll _usersBll;


        public LayoutController(IUsersBll usersBll)
        {
            _usersBll = usersBll;

        }


        [HttpGet]
        public async Task<JsonResult> GetUsersInfo()
        {
            string account = "";
            //1.先去得到登录信息
            object ob = Session["LoginOK"];
            if (ob == null)
            {
                HttpCookie uCookie = Request.Cookies["LoginOK"];
                account = uCookie.Value;
            }
            else
            {
                account = ob.ToString();
            }

            //2.得到当前账号下的所有信息
            var user = await _usersBll.GetUsersByEmail(account);

            UsersInfoViewModel ulvm = new UsersInfoViewModel
            {
                NickName = user.NickName,
                SmallImage = user.Image
            };

            return Json(ulvm, JsonRequestBehavior.AllowGet);
        }
    }
}