using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BlogSystem.MVCSite.Filter
{
    public class AdminAuthorizeAttribute : AuthorizeAttribute
    {
        //验证sesssion和cookie当中是否有值，有值的时候就是登入之后，没值的时候需要登录
        /// <summary>
        /// 没登录的情况下，跳转到登录界面
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.HttpContext.Response.Redirect("/backend/Login/SignIn");
        }

        /// <summary>
        /// 验证session和cookie判断是否登录，返回布尔值，true是登录，false是未登录
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            bool rs = (httpContext.Session["LoginOK"] != null && httpContext.Session["RolesId"] != null)
                || (httpContext.Request.Cookies["LoginOK"] != null && httpContext.Request.Cookies["RolesId"] != null);
            return rs;
        }
    }
}