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
    [AdminAuthorize]
    public class LayoutController : Controller
    {
        private IUsersBll _usersBll;
        private ISystemMenuBll _menuSvc;
        private IAdminsPermissionBll _permissionSvc;

        public LayoutController(IUsersBll usersBll, ISystemMenuBll menuSvc,IAdminsPermissionBll permissionSvc)
        {
            _usersBll = usersBll;
            _menuSvc = menuSvc;
            _permissionSvc = permissionSvc;
        }

        /// <summary>
        /// 左边菜单绑定
        /// </summary>
        /// <returns>对应的Json字符串</returns>
        public async Task<ActionResult> LeftMenuBind()
        {
            //1.获取当前登录用户的权限id
            Guid rid = Guid.Empty;
            object ob = Session["RolesId"];
            if (ob == null) //如果通过Session获取的值为空，就是点击了记住密码用Cookie登录
            {
                HttpCookie rCookie = Request.Cookies["RolesId"];
                rid = Guid.Parse(rCookie.Value);
            }
            else
            {
                rid = Guid.Parse(ob.ToString());
            }

            //2.通过权限id得到能看到的系统菜单id
            var permissions = await _permissionSvc.GetAdminsPermissionListByRolesId(rid);

            //3.得到当前数据库当中所有的系统菜单详情，之后根据上面的系统菜单id进行筛选
            var allMenu = await _menuSvc.GetSystemMenuListByTitle("");

            //var menus = (from sm in allMenu
            //       where (from ap in permissions
            //            select ap.SystemMenuId).Equals(sm.Id)
            //       select sm
            //    ).ToList();

            var menus = new List<SystemMenuDto>();
            for (int i = 0; i < allMenu.Count; i++)
            {
                for (int j = 0; j < permissions.Count; j++)
                {
                    if (allMenu[i].Id == permissions[j].SystemMenuId)
                    {
                        menus.Add(allMenu[i]);
                    }
                }
            }

            //4.在上面得到所有能看到菜单当中找出一级菜单
            var parents = menus.Where(m => m.ParentId == Guid.Empty).OrderBy(m => m.UpdateTime);
            //5.得到最后要往界面返回的集合内容
            List<LeftMenuListViewModel> list = new List<LeftMenuListViewModel>();

            foreach (var item in parents)
            {
                //1.通过一级菜单的id查找对应的子级菜单
                List<LeftMenuListViewModel> sonList = new List<LeftMenuListViewModel>();
                var sonData = from all in menus
                              where all.ParentId == item.Id
                              select all;
                foreach (var sonItem in sonData)
                {
                    LeftMenuListViewModel son = new LeftMenuListViewModel
                    {
                        Title = sonItem.Title,
                        Link = sonItem.Link,
                        Icon = sonItem.Icon
                    };
                    sonList.Add(son);
                }
                //下面填充的内容为直接展示的内容
                LeftMenuListViewModel lmlvm = new LeftMenuListViewModel
                {
                    Title = item.Title,
                    Icon = item.Icon,
                    Link = item.Link,
                    SonList = sonList
                };
                list.Add(lmlvm);
            }

            return Json(list,JsonRequestBehavior.AllowGet);
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