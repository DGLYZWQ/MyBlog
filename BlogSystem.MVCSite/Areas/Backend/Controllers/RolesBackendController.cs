using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Web.Mvc;
using BlogSystem.IBLL;
using BlogSystem.MVCSite.Areas.Backend.Common;
using BlogSystem.MVCSite.Areas.Backend.Data.Roles;
using log4net;
using log4net.Core;
using PagedList;

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
        //1. 每页展示多少条
        //2. 一共能分多少页
        public async Task<ActionResult> List(string Search="",int page = 1)
        {
            //注册日志
            ILog log = LogManager.GetLogger(typeof(RolesBackendController));
            //(1) 得到数据总条数
            var count = await _rolesBll.GetRolesCountAsync(Search);

            //(3) 设置每页要展示条数
            var pageSize = PageConfig.GetPageSize();

            var data = await _rolesBll.GetRolesList(Search, false);
            List<RolesListViewModel> list = new List<RolesListViewModel>();

            foreach (var item in data)
            {
                RolesListViewModel rlvm = new RolesListViewModel()
                {
                    Id = item.Id,
                    Title = item.Title,
                    UpdateTime = item.UpdateTime
                };
                list.Add(rlvm);
            }

            ViewBag.Search = Search;
            ViewBag.PageIndex = page;
            IPagedList<RolesListViewModel> pages = list.ToPagedList(page, PageConfig.GetPageSize());
            return View(pages);
        }
    }
}