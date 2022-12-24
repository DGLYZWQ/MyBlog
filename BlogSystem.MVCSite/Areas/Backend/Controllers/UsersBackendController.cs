using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Net.Mime;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Async;
using BlogSystem.IBLL;
using BlogSystem.MVCSite.Areas.Backend.Common;
using BlogSystem.MVCSite.Areas.Backend.Data.Users;
using BlogSystem.MVCSite.Filter;
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

                var names = UploadFiles(file, @"../../Upload/Users/"); //得到上传图片的名称

                var rs = await _users_bll.AddUsersAsync(model.Email, GetMD5String(model.Password), model.NickName, names[0],
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

        public string[] UploadFiles(HttpPostedFileBase file,string url)
        {
            if (!file.FileName.Equals(""))
            {
                Random r = new Random();
                var newName = DateTime.Now.ToString("yyyyMMddHHmmss")
                              + r.Next(1000, 10000)
                              + file.FileName.Substring(file.FileName.LastIndexOf('.'));
                var path = Server.MapPath(url + newName);

                file.SaveAs(path); // 保存的正常大小的图片

                ImageProcessingJob job = new ImageProcessingJob(); //实例化第三方缩略图插件
                job.Filters.Add(new FixedResizeConstraint(36, 36));
                //202207221511161234_sm.png
                var sm_name = newName.Substring(0, newName.LastIndexOf('.'))
                              + "_sm" + newName.Substring(newName.LastIndexOf('.'));
                var sm_path = Server.MapPath(url + sm_name);
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

        [HttpGet]
        public async Task<ActionResult> Edit(Guid id)
        {
            var data = await _users_bll.GetUsersById(id);

            await BindRoles(data.RolesId);

            return View(new EditUsersViewModel()
            {
                Id = data.Id,
                Email = data.Email,
                Password = data.Password,
                NickName = data.NickName,
                Avatar = data.Avatar,
                RolesId = data.RolesId,
                Image = data.Image
            });
        }

        [HttpPost]
        public async Task<ActionResult> Edit(EditUsersViewModel model)
        {
            if (ModelState.IsValid)
            {
                var file = Request.Files["MyPhoto"];
                var rs = -1;
                if (file.FileName != "" && file.FileName != null) //修改头像时
                {
                    var names = UploadFiles(file, @"../../../Upload/Users/");
                    rs = await _users_bll.EditUsersAsync(model.Id, model.Email, model.Password, model.NickName,
                        names[0], names[1], model.RolesId);
                }
                else
                {
                    rs = await _users_bll.EditUsersAsync(model.Id, model.Email, model.Password, model.NickName,
                        model.Avatar, model.Image, model.RolesId);
                }

                if (rs > 0)
                {
                    return Content("<script>alert('修改成功');location.href='../../../Backend/UsersBackend/List'</script>");
                }
            }

            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> Delete(Guid id)
        {
            var rs = await _users_bll.DeleteUsersAsync(id);
            if (rs > 0)
            {
                return Content("<script>alert('删除成功');location.href='../../../Backend/UsersBackend/List'</script>");
            }
            else if(rs == -2)
            {
                return Content("<script>alert('数据传输丢失，请稍后再试');location.href='../../../Backend/UsersBackend/List'</script>");
            }
            else
            {
                return Content("<script>alert('删除失败');location.href='../../../Backend/UsersBackend/List'</script>");
            }
        }
        /// <summary>
        /// 密码加密（MD5)
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string GetMD5String(string str)
        {
            //1.创建md5对象
            MD5 md5 = MD5.Create();
            //2.把要加密的字符转换为字节数组
            byte[] buffer = Encoding.UTF8.GetBytes(str);
            //3.把字节数组当中的所有字节进行加密，之后保存到新的字节数组当中
            byte[] newBuffer = md5.ComputeHash(buffer);
            //4.把加密后的字节数组转换为字符串
            var substring = new StringBuilder();
            foreach (var item in newBuffer)
            {
                substring.Append(item.ToString("x2"));
            }

            return substring.ToString();
        }

    }
}