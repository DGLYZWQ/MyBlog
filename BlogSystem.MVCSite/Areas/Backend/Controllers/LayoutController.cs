using System.Threading.Tasks;
using System.Web.Mvc;
using BlogSystem.IBLL;

namespace BlogSystem.MVCSite.Areas.Backend.Controllers
{
    public class LayoutController : Controller
    {
        private IUsersBll _usersBll;
        private ISystemMenuBll _menuSvc;

        public LayoutController(IUsersBll usersBll, ISystemMenuBll menuSvc)
        {
            _usersBll = usersBll;
            _menuSvc = menuSvc;
        }

        /// <summary>
        /// 左边菜单绑定
        /// </summary>
        /// <returns>对应的Json字符串</returns>
        public async Task<ActionResult> LeftMenuBind()
        {

        }

    }
}