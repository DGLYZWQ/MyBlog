using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using BlogSystem.IBLL;
using BlogSystem.MVCSite.Areas.Backend.Common;
using BlogSystem.MVCSite.Areas.Backend.Data.Users;
using PagedList;

namespace BlogSystem.MVCSite.Areas.Backend.Controllers
{
    public class UsersBackendController : Controller
    {
        private IUsersBll _users_bll;
        private IRolesBll _roles_bll;
        public UsersBackendController(IUsersBll users_bll,IRolesBll roles_bll)
        {
            _users_bll = users_bll;
            _roles_bll = roles_bll;
        }
        public async Task<ActionResult> List(string Search="", int page=1)
        {
            var data = await _users_bll.GetUsersByNickName(Search);
            List<UsersListViewModel> list=new List<UsersListViewModel>();
            foreach (var item in data)
            {
                //先去通过这个item对象查看对应的权限信息
                var role = await _roles_bll.GetRolesAsync(item.RolesId);
                UsersListViewModel ulvm = new UsersListViewModel()
                {
                    Id = item.Id,
                    Email = item.Email,
                    Password = item.Password,
                    NickName = item.NickName,
                    UpdateTime = item.UpdateTime,
                    Avatar = item.Avatar,
                    Image = item.Image,
                    RolesTitle = role.Title //把查询出来的权限名称放入
                };
                list.Add(ulvm);
            }
            ViewBag.Search = Search;
            ViewBag.PageIndex = page;
            IPagedList<UsersListViewModel> pages = list.ToPagedList(page,PageConfig.GetPageSize());
            return View(pages);
        }
    }
}