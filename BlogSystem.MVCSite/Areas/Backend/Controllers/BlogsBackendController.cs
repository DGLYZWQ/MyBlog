using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using BlogSystem.Dtos;
using BlogSystem.IBLL;
using BlogSystem.MVCSite.Areas.Backend.Common;
using BlogSystem.MVCSite.Areas.Backend.Data.Blogs;
using BlogSystem.MVCSite.Filter;
using PagedList;

namespace BlogSystem.MVCSite.Areas.Backend.Controllers
{
    [AdminAuthorize]
    public class BlogsBackendController : Controller
    {
        private IBlogBll _blog_bll;
        private ICategoryBll _category_bll;
        public BlogsBackendController(IBlogBll blog_bll,ICategoryBll category_bll)
        {
            _blog_bll = blog_bll;
            _category_bll = category_bll;
        }

        public async Task<ActionResult> List(string Search="", int page = 1)
        {
            var data = await _blog_bll.GetDataByTitleAsync(Search);
            List<BlogsListViewModel> list = new List<BlogsListViewModel>();
            foreach (var item in data)
            {
                BlogsListViewModel blvm = new BlogsListViewModel()
                {
                    Id = item.Id,
                    Title = item.Title,
                    Views = item.Views,
                    Comments = item.Comments,
                    CategoryName = await GetCategoryName(item.CategoryId),
                    IsPublic = item.IsPublic,
                    UpdateTime = item.UpdateTime,
                };
                list.Add(blvm);
            }

            ViewBag.Search = Search;
            ViewBag.PageIndex = page;
            IPagedList<BlogsListViewModel> pages = list.ToPagedList(page, PageConfig.GetPageSize());
            return View(pages);
        }

        public async Task<string> GetCategoryName(Guid categoryId)
        {
            var data = await _category_bll.GetCategoryByIdAsync(categoryId);
            return data?.Title;
        }

        [HttpGet]
        public async Task<ActionResult> Add()
        {
            // await BindCategory(Guid.Empty);
            var clist = await _category_bll.GetAllAsync();
            ViewBag.Category = new SelectList(clist.Where(x => x.Title == "公告").ToList(), "Id", "Title");
            return View(new AddNoticeViewModel());
        }

        public async Task BindCategory(Guid id)
        {
            var category = await _category_bll.GetCategoryByIdAsync(id);
            if (id == Guid.Empty)
            {
                SelectList categoryList = new SelectList(category.Title,"Id","Title","Description");
                ViewBag.CategoryName = categoryList;
            }
            else
            {
                SelectList categoryList = new SelectList(category.Title, "Id","Title","Description",id);
                ViewBag.CategoryName = categoryList;
            }
        }

        [HttpPost]
        public async Task<ActionResult> Add(AddNoticeViewModel model)
        {
            if (ModelState.IsValid)
            {
                BlogSystem.Dtos.UsersDto dto = Session["admin"] as BlogSystem.Dtos.UsersDto;
                if (dto == null) dto = new UsersDto();
                var rs = await _blog_bll.AddBlogAsync(model.Title, model.Content,model.CategoryId,dto.Id,true);
                if (rs > 0)
                {
                    return Content("<script>alert('添加成功');location.href='/Backend/BlogsBackend/List'</script>");
                }
            }
            var clist = await _category_bll.GetAllAsync();
            ViewBag.Category = new SelectList(clist.Where(x => x.Title == "公告").ToList(), "Id", "Title",model.CategoryId);

            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> Edit(Guid id)
        {
            var data = await _blog_bll.GetBlogByIdAsync(id);

            //await BindRoles(data.CategoryId);
            var clist = await _category_bll.GetAllAsync();
            ViewBag.Category = new SelectList(clist.ToList(), "Id", "Title",data.CategoryId);

            return View(new EditBlogViewModel()
            {
                BlogName = data.Title,
                BlogId = data.Id,
                CategoryId = data.CategoryId,
                IsPublic = data.IsPublic
            });
        }


        [HttpPost]
        public async Task<ActionResult> Edit(EditBlogViewModel model)
        {
            if (ModelState.IsValid)
            {
                var rs = await _blog_bll.EditAdminBlogAsync(model.BlogId, model.CategoryId, model.IsPublic);
                return Content("<script>alert('修改成功');location.href='/Backend/BlogsBackend/List'</script>");
            }

            return View(model);
        }
        [HttpGet]
        public async Task<ActionResult> Delete(Guid id)
        {
            var rs = await _blog_bll.DeleteBlogAsync(id);
            if (rs > 0)
            {
                return Content("<script>alert('删除成功');location.href='/Backend/BlogsBackend/List'</script>");
            }
            else if (rs == -2)
            {
                return Content("<script>alert('数据传输丢失，请稍后再试');location.href='/Backend/BlogsBackend/List'</script>");
            }
            else
            {
                return Content("<script>alert('删除失败');location.href='/Backend/BlogsBackend/List'</script>");
            }
        }

    }
}