using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using BlogSystem.IBLL;
using BlogSystem.MVCSite.Areas.Backend.Common;
using BlogSystem.MVCSite.Areas.Backend.Data.Category;
using BlogSystem.MVCSite.Areas.Backend.Views.CategoryBackend;
using PagedList;

namespace BlogSystem.MVCSite.Areas.Backend.Controllers
{
    public class CategoryBackendController : Controller
    {
        private ICategoryBll _category_bll;

        public CategoryBackendController(ICategoryBll category_bll)
        {
            _category_bll = category_bll;
        }
        public async Task<ActionResult> List(string Search="", int page = 1)
        {
            var data = await _category_bll.GetDataByTitleAsync(Search);
            List<CategoryListViewModel> list = new List<CategoryListViewModel>();
            foreach (var item in data)
            {
                CategoryListViewModel clvm = new CategoryListViewModel()
                {
                    Id = item.Id,
                    Title = item.Title,
                    Description = item.Description,
                    UpdateTime = item.UpdateTime
                };
                list.Add(clvm);
            }

            ViewBag.Search = Search;
            ViewBag.PageIndex = page;
            IPagedList<CategoryListViewModel> pages = list.ToPagedList(page, PageConfig.GetPageSize());
            return View(pages);
        }

        [HttpGet]
        public ActionResult Add()
        {
            return View(new AddCategoryViewModel());
        }

        [HttpPost]
        public async Task<ActionResult> Add(AddCategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var rs = await _category_bll.AddCategoryAsync(model.Title, model.Description);
                if (rs > 0)
                {
                    //return Content("<script>alert('新增成功');location.href='../../../Backend/CategoryBackend/List'</script>");
                    return RedirectToAction("List");
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<JsonResult> Validate(string Title)
        {
            var result = await _category_bll.IsExistsAsync(Title);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> Edit(Guid id)
        {
            var data = await _category_bll.GetCategoryByIdAsync(id);

            if (data != null)
            {
                return View(new EditCategoryViewModel
                {
                    Id = data.Id,
                    Title = data.Title,
                    Description = data.Description
                });
            }
            else
            {
                return RedirectToAction("List");
            }
        }

        [HttpPost]
        public async Task<ActionResult> Edit(EditCategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                int res = await _category_bll.EditCategoryAsync(model.Id, model.Title, model.Description);
                if (res > 0)
                    return RedirectToAction("List");
            }

            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> Delete(Guid id)
        {
            var rs = await _category_bll.DeleteCategoryAsync(id);
            if (rs > 0)
            {
                return Content("<script>alert('删除成功');location.href='../../../Backend/CategoryBackend/List'</script>");
            }
            else if (rs == -2)
            {
                return Content("<script>alert('数据传输丢失，请稍后再试');location.href='../../../Backend/CategoryBackend/List'</script>");
            }
            else
            {
                return Content("<script>alert('删除失败');location.href='../../../Backend/CategoryBackend/List'</script>");
            }
        }
    }
}