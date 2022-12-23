using BlogSystem.Dtos;
using BlogSystem.IBLL;
using BlogSystem.MVCSite.Areas.Backend.Common;
using BlogSystem.MVCSite.Areas.Backend.Data.Blogs;
using BlogSystem.MVCSite.Models.dtos;
using CodeCarvings.Piczard;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BlogSystem.MVCSite.Controllers
{
    public class FrontHomeController : Controller
    {

        private IBlogBll _blog_bll;
        private ICategoryBll _category_bll;
        private IUsersBll _users_bll;
        private readonly IRolesBll _rolesBll;
        private IUserFocusBll _userFocusBll;
        private IUserMsgBll _userMsgBll;
        private ICommentsBll _commentsBll;
        private IMessagesBll _messagesBll;
        public FrontHomeController(IBlogBll blog_bll, ICategoryBll category_bll, 
            IUsersBll users_bll, IRolesBll rolesBll, IUserFocusBll userFocusBll, 
            IUserMsgBll userMsgBll, ICommentsBll commentsBll, IMessagesBll messagesBll)
        {
            _blog_bll = blog_bll;
            _category_bll = category_bll;
            _users_bll = users_bll;
            _rolesBll = rolesBll;
            _userFocusBll = userFocusBll;
            _userMsgBll = userMsgBll;
            _commentsBll = commentsBll;
            _messagesBll = messagesBll;
        }

        public ActionResult Login()
        {
            return View(new Areas.Backend.Data.Login.LoginViewModel());
        }
        [HttpPost]
        public async Task<ActionResult> Login(Areas.Backend.Data.Login.LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var data = await _users_bll.LoginAsync(model.Email, model.Password);
                
                if (data != null)
                {
                    var rolesList = await _rolesBll.GetRolesListByPageAsync(1, 1, "用户", true);
                    var RolesId = rolesList.First().Id;
                    if (data.RolesId==RolesId)
                    {
                        Session["user"] = data;

                        return RedirectToAction("Index", "FrontHome");
                    }
                    else
                    {
                        ModelState.AddModelError("Password", "用户名或者密码错误");
                    }
                      
                }
                else
                {
                    ModelState.AddModelError("Password", "用户名或者密码错误");
                }
            }

            return View(model);
        }

        public ActionResult Register()
        {
            return View(new RegisterViewModel());
        }
        [HttpPost]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if(ModelState.IsValid)
            {
                var rolesList = await _rolesBll.GetRolesListByPageAsync(1, 1, "用户", true);
                var RolesId = rolesList.First().Id;
                var info = await _users_bll.GetUsersByEmail(model.Email);
                if (info != null)
                {
                    ModelState.AddModelError("Email", "电子邮箱已存在");
                    return View(model);
                }
                await _users_bll.RegisterAsync(model.Email, model.Password, RolesId,model.Email);
                return Content("<script>alert('注册成功');location.href='/FrontHome/Login'</script>");
            }
            return View(model);
        }

        public ActionResult ForgetPassword()
        {
            return View();
        }
        [HttpPost]
        public  ActionResult ForgetPassword(ForgetPasswordViewModel model)
        {
            return View("ChangePassword",new RegisterViewModel { Email = model.Email });
            
        }
        [HttpPost]
        public async Task<ActionResult> ChangePassword(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var data = await _users_bll.ResetPwd(model.Email, model.Password);
                if (data > 0)
                {


                    return Content("<script>alert('密码重置成功');location.href='/FrontHome/Login'</script>");
                }
            }

            return View(model);
        }
        
        public async Task<JsonResult> CheckEmailAsync(string Email)
        {
            var rs = await _users_bll.IsExists(Email);
            return Json(!rs, JsonRequestBehavior.AllowGet);
        }
        public async Task<JsonResult> CheckEmailUpdateAsync(string Email)
        {
            var user = Session["user"] as UsersDto;
            if (user != null)
            {
                var rs = await _users_bll.IsExists(Email, user.Id);
                return Json(!rs, JsonRequestBehavior.AllowGet);
            }

            return Json(false, JsonRequestBehavior.AllowGet);
        }
        public async Task<JsonResult> CheckEmailIsExit(string Email)
        {
            var rs = await _users_bll.IsExists(Email);
            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> Index(string Search = "", int page = 1)
        {
            var data = await _blog_bll.GetDataByTitleAsync(Search);
            List<BlogsListViewModel> list = new List<BlogsListViewModel>();
            foreach (var item in data.Where(x=>x.IsPublic))
            {
                BlogsListViewModel blvm = new BlogsListViewModel()
                {
                    Id = item.Id,
                    Title = item.Title,
                    Views = item.Views,
                    Comments = item.Comments,
                    CategoryName = await GetCategoryName(item.CategoryId),
                    UserName = await GetUserName(item.UsersId),
                    UsersId = item.UsersId,
                    IsPublic = item.IsPublic,
                    UpdateTime = item.UpdateTime,
                    Content=GetSubString(LostHTML(item.Content),150,"...")
                };
                list.Add(blvm);
            }

            ViewBag.Search = Search;
            ViewBag.PageIndex = page;
            IPagedList<BlogsListViewModel> pages = list.ToPagedList(page, 4);
             
            return View(pages);
        }
        public async Task<ActionResult> MyBlog(string Search = "", int page = 1)
        {
            var user = Session["user"] as UsersDto;
            if (user == null) return Redirect("/FrontHome/Login");
            var data = await _blog_bll.GetMyBlogListAsync(user.Id,"",Search);
            List<BlogsListViewModel> list = new List<BlogsListViewModel>();
            foreach (var item in data.Where(x => x.IsPublic))
            {
                BlogsListViewModel blvm = new BlogsListViewModel()
                {
                    Id = item.Id,
                    Title = item.Title,
                    Views = item.Views,
                    Comments = item.Comments,
                    CategoryName = await GetCategoryName(item.CategoryId),
                    UserName = await GetUserName(item.UsersId),
                    UsersId = item.UsersId,
                    IsPublic = item.IsPublic,
                    UpdateTime = item.UpdateTime,
                    Content = GetSubString(LostHTML(item.Content), 150, "...")
                };
                list.Add(blvm);
            }

            ViewBag.Search = Search;
            ViewBag.PageIndex = page;
            IPagedList<BlogsListViewModel> pages = list.ToPagedList(page, 4);

            return View(pages);
        }
        public async Task<ActionResult> DelBlog(Guid id)
        {
            var user = Session["user"] as UsersDto;
            if (user == null) return Redirect("/FrontHome/Login");
            await _blog_bll.DeleteBlogAsync(id);

            return Redirect("/FrontHome/MyBlog");
        }
        public async Task<ActionResult> EditBlog(Guid? id)
        {
            var list = await _category_bll.GetAllAsync();
            if (id!=null)
            {
              
                var blog = await _blog_bll.GetBlogByIdAsync(id.Value);
                ViewBag.SelectList = new SelectList(list, "Id", "Title", blog.CategoryId);
                return View(blog);
            }
            ViewBag.SelectList = new SelectList(list, "Id", "Title");
            return View(new BlogDto());
        }
        public async Task<ActionResult> UpdateInfo()
        {
            var user = Session["user"] as UsersDto;
            if (user == null) return Redirect("/FrontHome/Login");
            var u =await _users_bll.GetUsersById(user.Id);
            return View(new UpdateUsersDto { 
             Id=u.Id,
             Image=u.Image,
             Intro=u.Intro,
             Avatar=u.Avatar,
             Email=u.Email,
             NickName=u.NickName,
             Password=u.Password
            });
        }
        public string UploadFiles(HttpPostedFileBase file, string url)
        {
            if (file.FileName != "" && file.FileName != null) //修改头像时
            {

                Random r = new Random();
                var newName = DateTime.Now.ToString("yyyyMMddHHmmss")
                              + r.Next(1000, 10000)
                              + file.FileName.Substring(file.FileName.LastIndexOf('.'));
                var path = Server.MapPath(url + newName);

                file.SaveAs(path); // 保存的正常大小的图片

                return newName;
            }
            return "";
        }
        [HttpPost]
        public async Task<ActionResult> UpdateInfo(UpdateUsersDto model)
        {
            var user = Session["user"] as UsersDto;
            if (user == null) return Redirect("/FrontHome/Login");
            if (ModelState.IsValid)
            {
                var file = Request.Files["MyPhoto"];
                var file2 = Request.Files["Avatar"];
                var imgage= UploadFiles(file, @"/Upload/Users/"); 
                var avtar= UploadFiles(file2, @"/Upload/Users/");

                if (!string.IsNullOrWhiteSpace(imgage)) model.Image = imgage;
                if (!string.IsNullOrWhiteSpace(avtar)) model.Avatar = avtar;

                var rs =await _users_bll.UpdateInfo(user.Id, model.Email, model.Password, model.NickName, model.Avatar, model.Image, model.Intro);

                if (rs > 0)
                {
                    return Content("<script>alert('修改成功');location.href='/FrontHome/UserCenter?id="+user.Id+ "'</script>");
                }
            }

            return View(model);
        }
        [HttpPost]
        [ValidateInput(false)]
        public async Task<ActionResult> EditBlog(Guid? id,BlogDto model)
        {
            var user = Session["user"] as UsersDto;
            if (user == null) return Redirect("/FrontHome/Login");
            if (id != null)
            {
                await _blog_bll.EditBlogAsync(id.Value, model.Title, model.CategoryId, model.Content);
            }
            else
            {
                await _blog_bll.AddBlogAsync(model.Title, model.Content, model.CategoryId, user.Id);
            }

            return Redirect("/FrontHome/MyBlog");
        }
        public async Task<ActionResult> CategoryBlogList(Guid cid, string Search = "", int page = 1)
        {
            //var categoryId = Request.QueryString["cid"];
            //var cid = Guid.Parse(categoryId);
            var data = await _blog_bll.GetDataByCategoryTitleAsync(cid,Search);
            var categoryTask = await _category_bll.GetCategoryByIdAsync(cid);
            List<BlogsListViewModel> list = new List<BlogsListViewModel>();
            foreach (var item in data.Where(x => x.IsPublic))
            {
                BlogsListViewModel blvm = new BlogsListViewModel()
                {
                    Id = item.Id,
                    Title = item.Title,
                    Views = item.Views,
                    Comments = item.Comments,
                    CategoryName = await GetCategoryName(item.CategoryId),
                    UserName=await GetUserName(item.UsersId),
                    UsersId=item.UsersId,
                    IsPublic = item.IsPublic,
                    UpdateTime = item.UpdateTime,
                    Content = GetSubString(LostHTML(item.Content), 150, "...")
                };
                list.Add(blvm);
            }

            ViewBag.Search = Search;
            ViewBag.PageIndex = page;
            ViewBag.Category =  categoryTask;
            IPagedList<BlogsListViewModel> pages = list.ToPagedList(page, 4);

            return View(pages);
        }
        public async Task<ActionResult> FocusBlogList(string Search = "", int page = 1)
        {
            var user = Session["user"] as UsersDto;
            if (user == null) return Redirect("/FrontHome/Login");
            
            var data = await _blog_bll.GetFocusListAsync(user.Id, Search);
           
            List<BlogsListViewModel> list = new List<BlogsListViewModel>();
            foreach (var item in data.Where(x => x.IsPublic))
            {
                BlogsListViewModel blvm = new BlogsListViewModel()
                {
                    Id = item.Id,
                    Title = item.Title,
                    Views = item.Views,
                    Comments = item.Comments,
                    CategoryName = await GetCategoryName(item.CategoryId),
                    UserName = await GetUserName(item.UsersId),
                    UsersId = item.UsersId,
                    IsPublic = item.IsPublic,
                    UpdateTime = item.UpdateTime,
                    Content = GetSubString(LostHTML(item.Content), 150, "...")
                };
                list.Add(blvm);
            }

            ViewBag.Search = Search;
            ViewBag.PageIndex = page;
           
            IPagedList<BlogsListViewModel> pages = list.ToPagedList(page, 4);

            return View(pages);
        }
        public async Task<ActionResult> BlogDetail(Guid id)
        {
            var blog = await _blog_bll.GetBlogByIdAsync(id);
            var categoryGroupList = await _blog_bll.GetCategroyGroupAsync(blog.UsersId);
            foreach (var item in categoryGroupList)
            {
                item.CategoryName = await GetCategoryName(item.CategoryId);
            }
            ViewBag.BlogCategroy = categoryGroupList;
            var fanscount = await _userFocusBll.GetBeFocusCount(blog.UsersId);
            var focuscount = await _userFocusBll.GetFocusCount(blog.UsersId);
            var isFocus = false;
            var user = Session["user"] as UsersDto;
            if (user != null)
            {
                isFocus = await _userFocusBll.IsFocus(user.Id, blog.UsersId);
            }
            ViewBag.fanscount = fanscount;
            ViewBag.focuscount = focuscount;
            ViewBag.isFocus = isFocus;
            var blogUser = await _users_bll.GetUsersById(blog.UsersId);
            ViewBag.blogUser = blogUser;
            var commentcounts = await _commentsBll.GetCount(blog.Id);
            ViewBag.comments = commentcounts;
            //增加点击数
            string uid = "";

            if (user != null) uid = user.Id.ToString();
            await _blog_bll.Click(id, Request.UserHostAddress, uid);

            return View(blog);
        }
         
        public async Task<ActionResult> Comment(Guid id)
        {
            var list = await _commentsBll.GetAllAsync(id);
            foreach(var item in list)
            {
                item.UserName = await GetUserName(item.UserId);
            }
            return View(list);
        }
        [HttpPost]
        public async Task<ActionResult> CommentPost(Guid blogId,string msg)
        {
            var user = Session["user"] as UsersDto;
            if (user == null) return Redirect("/FrontHome/Login");
            await _commentsBll.AddCommentAsync(blogId, user.Id, msg);
            
            return Redirect("/FrontHome/Comment?id="+blogId);
        }

        public async Task<ActionResult> Focus(Guid uid,Guid bid)
        {
            var user = Session["user"] as UsersDto;
            if (user == null) return Redirect("/FrontHome/Login");

            var res = await _userFocusBll.Focus(user.Id, uid);
            if(res>0)
            {
                return Redirect("/FrontHome/BlogDetail?id="+bid);
            }

            return View();
        }
        public async Task<ActionResult> Message(int page=1)
        {
            var user = Session["user"] as UsersDto;
            if (user == null) return Redirect("/FrontHome/Login");
            List<UserMsgDto> list =await _userMsgBll.GetList(user.Id);
             

            
            ViewBag.PageIndex = page;
            IPagedList<UserMsgDto> pages = list.ToPagedList(page, 8);

            return View(pages);
        }
        public async Task<string> GetCategoryName(Guid categoryId)
        {
            var data = await _category_bll.GetCategoryByIdAsync(categoryId);
            return data?.Title;
        }
        public async Task<string> GetUserName(Guid userId)
        {
            var data = await _users_bll.GetUsersById(userId);
            return data?.Email;
        }
        public async Task<ActionResult> Category()
        {
            return View(await _category_bll.GetAllAsync());
        }
        public ActionResult AboutUs()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }
        public ActionResult Exit()
        {
            Session.Remove("user");
            return Redirect("/FrontHome/Login");
        }
        
        public ActionResult ContactUs()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> ContactUs(MessagesDto model)
        {
            await  _messagesBll.AddMessageAsync(model.Name, model.Email, model.Tel, model.Content);
            return Content("<script>alert('留言成功');location.href='/FrontHome/ContactUs'</script>");
        }
        public async Task<ActionResult> UserCenter(Guid id, int page = 1)
        {
            var cid = Request.QueryString["cid"]??"";
            var data = await _blog_bll.GetMyBlogListAsync(id,cid, "");
            List<BlogsListViewModel> list = new List<BlogsListViewModel>();
            foreach (var item in data.Where(x => x.IsPublic))
            {
                BlogsListViewModel blvm = new BlogsListViewModel()
                {
                    Id = item.Id,
                    Title = item.Title,
                    Views = item.Views,
                    Comments = item.Comments,
                    CategoryName = await GetCategoryName(item.CategoryId),
                    UserName = await GetUserName(item.UsersId),
                    UsersId = item.UsersId,
                    IsPublic = item.IsPublic,
                    UpdateTime = item.UpdateTime,
                    Content = GetSubString(LostHTML(item.Content), 150, "...")
                };
                list.Add(blvm);
            }

            
            ViewBag.PageIndex = page;
            IPagedList<BlogsListViewModel> pages = list.ToPagedList(page, 8);
            var UsersId = id;
            var categoryGroupList = await _blog_bll.GetCategroyGroupAsync(UsersId);
            foreach (var item in categoryGroupList)
            {
                item.CategoryName = await GetCategoryName(item.CategoryId);
            }
            ViewBag.BlogCategroy = categoryGroupList;
            var focuscount = await _userFocusBll.GetFocusCount(UsersId);
            var isFocus = false;
            var user = Session["user"] as UsersDto;
            if (user != null)
            {
                isFocus = await _userFocusBll.IsFocus(user.Id, UsersId);
            }
           
            ViewBag.focuscount = focuscount;
            ViewBag.isFocus = isFocus;
            var blogUser = await _users_bll.GetUsersById(UsersId);
            ViewBag.blogUser = blogUser;

            return View(pages);
        }
        
        /// <summary>
        /// 过滤字符串中的html代码
        /// </summary>
        /// <param name="Str"></param>
        /// <returns>返回过滤之后的字符串</returns>
        public static string LostHTML(string Str)
        {
            string Re_Str = "";
            if (Str != null)
            {
                if (Str != string.Empty)
                {
                    string Pattern = "<\\/*[^<>]*>";
                    Re_Str = Regex.Replace(Str, Pattern, "");
                }
            }
            return (Re_Str.Replace("\\r\\n", "")).Replace("\\r", "");
        }
        /// <summary>
        /// 截取字符串函数
        /// </summary>
        /// <param name="Str">所要截取的字符串</param>
        /// <param name="Num">截取字符串的长度</param>
        /// <param name="Num">截取字符串后省略部分的字符串</param>
        /// <returns></returns>
        public string GetSubString(string Str, int Num, string LastStr)
        {
            return (Str.Length > Num) ? Str.Substring(0, Num) + LastStr : Str;
        }
    }
}