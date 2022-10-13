using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using BlogSystem.IBLL;
using BlogSystem.MVCSite.Areas.Backend.Data.AdminsPermission;
using BlogSystem.MVCSite.Areas.Backend.Data.Roles;
using BlogSystem.MVCSite.Areas.Backend.Data.SystemMenu;

namespace BlogSystem.MVCSite.Areas.Backend.Controllers
{
    public class AdminsPermissionBackendController : Controller
    {
        private IRolesBll _rolesSvc;
        private ISystemMenuBll _menuSvc;
        private IAdminsPermissionBll _permissionSvc;

        public AdminsPermissionBackendController(IRolesBll rolesSvc, ISystemMenuBll menuSvc, IAdminsPermissionBll permissionSvc)
        {
            _rolesSvc = rolesSvc;
            _menuSvc = menuSvc;
            _permissionSvc = permissionSvc;
        }

        [HttpGet]
        public async Task<ActionResult> List()
        {
            await RolesListBind(Guid.Empty);
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> List(RolesListViewModel roles)
        {
            await RolesListBind(roles.Id);   //下拉列表的绑定

            var permissionList = await _permissionSvc.GetAdminsPermissionListByRolesId(roles.Id);
            var data = await _menuSvc.GetSystemMenuListByTitle("");  //查询所有的系统菜单
            var allMenuList = new List<SystemMenuListViewModel>();
            foreach (var item in data)
            {
                SystemMenuListViewModel smvm = new SystemMenuListViewModel
                {
                    Id = item.Id,
                    Title = item.Title
                };
                allMenuList.Add(smvm);
            }


            var haveData = (from sm in allMenuList
                            where (from ap in permissionList select ap.SystemMenuId).Contains(sm.Id)
                            select sm).ToList();

            var noneHave = (from sm in allMenuList
                            where !(from ap in permissionList select ap.SystemMenuId).Contains(sm.Id)
                            select sm).ToList();

            ViewBag.HaveList = haveData;
            ViewBag.NoHave = noneHave;

            return View();

        }

        private async Task RolesListBind(Guid selectedId)
        {
            var data = await _rolesSvc.GetRolesList("", true);
            var rolesList = new List<RolesListViewModel>();
            foreach (var item in data)
            {
                RolesListViewModel rlvm = new RolesListViewModel
                {
                    Id = item.Id,
                    Title = item.Title,
                };
                rolesList.Add(rlvm);
            }
            if (selectedId == Guid.Empty)
            {
                SelectList sl = new SelectList(rolesList, "Id", "Title");
                ViewBag.Roles = sl;
            }
            else
            {
                SelectList sl = new SelectList(rolesList, "Id", "Title", selectedId);
                ViewBag.Roles = sl;
                ViewBag.RolesId = selectedId;
            }
        }


        [HttpPost]
        public async Task<ActionResult> Add(AddAdminsPermissionViewModel model)
        {
            if (ModelState.IsValid)
            {
                foreach (var menuId in model.SystemMenuId)
                {
                    await _permissionSvc.AddAdminsPermission(model.RolesId, menuId);
                }
            }

            return RedirectToAction("List", "AdminsPermissionBackend");
        }


        [HttpPost]
        public async Task<ActionResult> Delete(DeleteAdminsPermissionViewModel model)
        {
            if (ModelState.IsValid)
            {
                foreach (var item in model.SystemMenuId)
                {
                    await _permissionSvc.DeleteAdminsPermission(model.RolesId, item);
                }
            }

            return RedirectToAction("List", "AdminsPermissionBackend");
        }
    }
}