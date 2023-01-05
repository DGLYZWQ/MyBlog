using System;
using System.Collections.Generic;
using System.Web.Mvc;
using BlogSystem.MVCSite.Filter;
using System.Linq;
using System.Threading.Tasks;
using System.Web.UI.WebControls.Expressions;
using BlogSystem.Dtos;
using BlogSystem.MVCSite.Areas.Backend.Common;
using BlogSystem.MVCSite.Areas.Backend.Data.Messages;
using PagedList;

namespace BlogSystem.MVCSite.Areas.Backend.Controllers
{
    [AdminAuthorize]
    public class HomeController : Controller
    {
        private IBlogBll _blogBll;
        private IMessagesBll _messagesBll;
        private ICommentsBll _commentsBll;
        public HomeController(IBlogBll blogBll,IMessagesBll messagesBll,ICommentsBll commentsBll)
        {
            _blogBll = blogBll;
            _messagesBll = messagesBll;
            _commentsBll = commentsBll;
        }
        // GET
        public async Task<ActionResult> Index()
        {
            List<string> xdatas = new List<string>();
            List<int> ydatas1 = new List<int>();
            List<int> ydatas2 = new List<int>();
            List<int> ydatas3 = new List<int>();
            int y1 = 0, y2 = 0, y3 = 0;
            int y1total1 =await _blogBll.GetViewsAllCount();
            int y1tota2 =await _commentsBll.GetAllCount();
            int y1tota3 =await _messagesBll.GetViewsAllCount();
            for (int i=6;i>=0;i--)
            {
                var date = DateTime.Now.AddDays(-i).ToString("yyyy-MM-dd");
                xdatas.Add(date);
                var y1count =await _blogBll.GetViewsCount(DateTime.Parse(date+" 00:00:00"), DateTime.Parse(date + " 23:59:59"));
                var y2count = await _commentsBll.GetCount(DateTime.Parse(date+" 00:00:00"), DateTime.Parse(date + " 23:59:59"));
                var y3count = await _messagesBll.GetViewsCount(DateTime.Parse(date+" 00:00:00"), DateTime.Parse(date + " 23:59:59"));

                ydatas1.Add(y1count);
                ydatas2.Add(y2count);
                ydatas3.Add(y3count);

                y1 += y1count;
                y2 += y2count;
                y3 += y3count;

            }
            ViewBag.Data = new IndexDto
            {
                y1 = y1,
                y2 = y2,
                y3 = y3,
                ytotal1 = y1total1,
                ytotal2 = y1tota2,
                ytotal3 = y1tota3,
                xdatas = xdatas,
                ydatas1 = ydatas1,
                ydatas2 = ydatas2,
                ydatas3 = ydatas3
            };
            return View();
        }
        public ActionResult Logout()
        {
            Session.Remove("admin");
            Session.Remove("LoginOK");
            Session.Remove("RolesId");
            return Redirect("/Backend/Login/SignIn");
        }

        public async Task<ActionResult> AdminInfo()
        {
            return View();
        }

        public async Task<ActionResult> HowToUse()
        {
            return View();
        }

        public async Task<ActionResult> FAQ()
        {
            return View();
        }
    }

    public class IndexDto
    {
        public int y1 { get; set; } 
        public int y2 { get; set; } 
        public int y3 { get; set; }
        public int ytotal1 { get; set; }
        public int ytotal2 { get; set; }
        public int ytotal3 { get; set; }
        public List<string> xdatas { get; set; }
        public List<int> ydatas1 { get; set; }
        public List<int> ydatas2 { get; set; }
        public List<int> ydatas3 { get; set; }
    }
}