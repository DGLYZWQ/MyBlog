﻿using System.Collections.Generic;
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
    }
}