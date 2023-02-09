using BlogSystem.IBLL;
using Come.CollectiveOAuth.Models;
using Come.CollectiveOAuth.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Come.Web.Sample.Controllers
{
    public class OAuth2Controller : Controller
    {
        private IUsersBll _users_bll;
        private IRolesBll _rolesBll;
        public OAuth2Controller(IUsersBll users_bll,IRolesBll rolesBll)
        {
            _users_bll = users_bll;
            _rolesBll = rolesBll;
        }
        /// <summary>
        /// 构建授权Url方法
        /// </summary>
        /// <param name="authSource"></param>
        /// <returns>RedirectUrl</returns>
        public ActionResult Authorization(string authSource)
        {
            AuthRequestFactory authRequest = new AuthRequestFactory();
            var request = authRequest.getRequest(authSource);
            var authorize = request.authorize(AuthStateUtils.createState());
            Console.WriteLine(authorize);
            return Redirect(authorize);
        }

        /// <summary>
        /// 授权回调方法
        /// </summary>
        /// <param name="authSource"></param>
        /// <param name="authCallback"></param>
        /// <returns></returns>
        public ActionResult Callback(string authSource, AuthCallback authCallback)
        {
            AuthRequestFactory authRequest = new AuthRequestFactory();
            var request = authRequest.getRequest(authSource);
            var authResponse = request.login(authCallback);
            var data = authResponse.data;
            var entity = data as AuthUser;
            var user = _users_bll.FindOne(entity.uuid, authSource);
            if(user==null)
            {
                var rolesList = _rolesBll.GetRolesByTitle("用户");
                var RolesId = rolesList.Id;
                user = _users_bll.RegisterAuto(entity.username+"@"+authSource+".com", BlogSystem.MVCSite.Models.MD5Helper.GetMD5String("123456"), entity.uuid, authSource, RolesId, entity.username);
            }
            Session["user"] = user;
            return Redirect("/FrontHome/Index");
            //return Content(JsonConvert.SerializeObject(authResponse));
        }

        /// <summary>
        /// 钉钉callback
        /// </summary>
        /// <param name="authSource"></param>
        /// <param name="authCallback"></param>
        /// <returns></returns>
        public ActionResult DingTalkCallback(AuthCallback authCallback)
        {
            AuthRequestFactory authRequest = new AuthRequestFactory();
            var request = authRequest.getRequest("DINGTALK_SCAN");
            var authResponse = request.login(authCallback);
            return Content(JsonConvert.SerializeObject(authResponse));
        }
    }
}