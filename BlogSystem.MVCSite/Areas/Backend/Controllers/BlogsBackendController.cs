using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using BlogSystem.Dtos;
using BlogSystem.IBLL;
using BlogSystem.MVCSite.Areas.Backend.Common;
using BlogSystem.MVCSite.Areas.Backend.Data.Blogs;
using PagedList;

namespace BlogSystem.MVCSite.Areas.Backend.Controllers
{
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
                    //CategoryName = await GetCategoryName(item.CategoryId),
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
            return data.Title;
        }
    }
}