﻿using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using BlogSystem.IBLL;
using BlogSystem.MVCSite.Areas.Backend.Common;
using BlogSystem.MVCSite.Areas.Backend.Data.Users;
using CodeCarvings.Piczard;
using PagedList;

namespace BlogSystem.MVCSite.Areas.Backend.Controllers
{
    public class UsersBackendController : Controller
    {
        private IUsersBll _users_bll;
        private IRolesBll _roles_bll;

        public UsersBackendController(IUsersBll users_bll, IRolesBll roles_bll)
        {
            _users_bll = users_bll;
            _roles_bll = roles_bll;
        }

        public async Task<ActionResult> List(string Search = "", int page = 1)
        {
            var data = await _users_bll.GetUsersByNickName(Search);
            List<UsersListViewModel> list = new List<UsersListViewModel>();
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
            IPagedList<UsersListViewModel> pages = list.ToPagedList(page, PageConfig.GetPageSize());
            return View(pages);
        }

        [HttpGet]
        public async Task<ActionResult> Add()
        {
            await BindRoles(Guid.Empty);
            return View(new AddUsersViewModel());
        }

        [HttpPost]
        public async Task<ActionResult> Add(AddUsersViewModel model)
        {
            if (ModelState.IsValid)
            {
                //获取表单传递过来的数据，并且实现新增功能
                var file = Request.Files["MyPhoto"];

                var names = UploadFiles(file); //得到上传图片的名称

                var rs = await _users_bll.AddUsersAsync(model.Email, model.Password, model.NickName, names[0],
                    names[1], model.RolesId);
                if (rs > 0)
                {
                    return Content("<script>alert('新增成功');location.href='../../../Backend/UsersBackend/List'</script>");
                }
            }

            return View(model);
        }


        /// <summary>
        /// 绑定权限下拉列表的
        /// </summary>
        /// <param name="id">当前选中的值</param>
        /// <returns></returns>
        public async Task BindRoles(Guid id)
        {
            //查询所有的权限信息
            var roles = await _roles_bll.GetRolesList("", true);
            //把集合传递给视图
            if (id == Guid.Empty)
            {
                SelectList rolesList = new SelectList(roles, "Id", "Title");
                ViewBag.Roleslist = rolesList;
            }
            else
            {
                SelectList rolesList = new SelectList(roles, "Id", "Title", id);
                ViewBag.Roleslist = rolesList;
            }
        }

        public string[] UploadFiles(HttpPostedFileBase file)
        {
            if (!file.FileName.Equals(""))
            {
                Random r = new Random();
                var newName = DateTime.Now.ToString("yyyyMMddHHmmss")
                              + r.Next(1000, 10000)
                              + file.FileName.Substring(file.FileName.LastIndexOf('.'));
                var path = Server.MapPath(@"../../Upload/Users/" + newName);

                file.SaveAs(path); // 保存的正常大小的图片

                ImageProcessingJob job = new ImageProcessingJob(); //实例化第三方缩略图插件
                job.Filters.Add(new FixedResizeConstraint(24, 24));
                //202207221511161234_sm.png
                var sm_name = newName.Substring(0, newName.LastIndexOf('.'))
                              + "_sm" + newName.Substring(newName.LastIndexOf('.'));
                var sm_path = Server.MapPath(@"../../Upload/Users/" + sm_name);
                job.SaveProcessedImageToFileSystem(path, sm_path);
                return new string[] {newName, sm_name};
            }

            return new string[] {"default.jpeg", "default.png"};
        }


        public async Task<JsonResult> CheckEmailAsync(string Email)
        {
            var rs = await _users_bll.IsExists(Email);
            return Json(!rs, JsonRequestBehavior.AllowGet);
        }
    }
}