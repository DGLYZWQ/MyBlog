using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.UI.WebControls.Expressions;
using BlogSystem.Dtos;
using BlogSystem.MVCSite.Areas.Backend.Common;
using BlogSystem.MVCSite.Areas.Backend.Data.Messages;
using PagedList;

namespace BlogSystem.MVCSite.Areas.Backend.Controllers
{
    public class MessagesBackendController : Controller
    {
        private IMessagesBll _messages_bll;

        public MessagesBackendController(IMessagesBll messages_bll)
        {
            _messages_bll = messages_bll;
        }

        [HttpGet]
        public async Task<ActionResult> List(string Search="", int page = 1)
        {
            var data = await _messages_bll.GetAllAsync(); 
            List<MessagesListViewModel> list = new List<MessagesListViewModel>();
            foreach (var item in data)
            {
                MessagesListViewModel mlvm = new MessagesListViewModel()
                {
                    Id = item.Id,
                    Name = item.Name,
                    Email = item.Email,
                    Content = item.Content,
                    IsRead = item.IsRead,
                    UpdateTime = item.UpdateTime
                };
                list.Add(mlvm);
            }

            ViewBag.Search = Search;
            ViewBag.PageIndex = page;
            IPagedList<MessagesListViewModel> pages = list.ToPagedList(page, PageConfig.GetPageSize());
            return View(pages);
        }
        public async Task<ActionResult> Check(Guid id)
        {
            var rs = await _messages_bll.Read(id, true);
            if (rs > 0)
            {
                return Content("<script>alert('已标记为已读');location.href='/Backend/MessagesBackend/List'</script>");
            }
            return View();
        }
        public async Task<ActionResult> Delete(Guid id)
        {
            var rs = await _messages_bll.DeleteMessageAsync(id);
            if (rs > 0)
            {
                return Content("<script>alert('删除成功');location.href='/Backend/MessagesBackend/List'</script>");
            }
            return View();
        }
    }
}