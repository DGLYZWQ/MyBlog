using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.UI.WebControls.Expressions;
using BlogSystem.Dtos;
using BlogSystem.MVCSite.Areas.Backend.Common;
using BlogSystem.MVCSite.Areas.Backend.Data.Comments;
using PagedList;

namespace BlogSystem.MVCSite.Areas.Backend.Controllers
{
    public class CommentsBackendController : Controller
    {
        private ICommentsBll _comments_bll;
        private IBlogBll _blog_bll;

        public CommentsBackendController(ICommentsBll comments_bll, IBlogBll blog_bll)
        {
            _comments_bll = comments_bll;
            _blog_bll = blog_bll;
        }
        public async Task<ActionResult> List(string Search="",int page = 1)
        {
            var data = await _comments_bll.GetAllAsync();
            List<CommentsListViewModel> list = new List<CommentsListViewModel>();
            foreach (var item in data)
            {
                var bid = await _blog_bll.GetBlogByIdAsync(item.BlogId);
                CommentsListViewModel clvm = new CommentsListViewModel()
                {
                    Id = item.Id,
                    Content = item.Content,
                    BlogName = bid.Title,
                    IsChecked = item.IsChecked,
                    UpdateTime = item.UpdateTime
                };
                list.Add(clvm);
            }

            ViewBag.Search = Search;
            ViewBag.PageIndex = page;
            IPagedList<CommentsListViewModel> pages = list.ToPagedList(page, PageConfig.GetPageSize());
            return View(pages);
        }
    }
}