using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using BlogSystem.Dtos;
using BlogSystem.IBLL;
using BlogSystem.IDAL;
using BlogSystem.Models;

namespace BlogSystem.BLL
{
    public class BlogBll : IBlogBll
    {
        private IBlogDal _dal;
        private ICommentsDal _commentsDal;
        private IViewsDal _viewsDal;
        private IUserFocusDal _focusDal;
        private ICategoryBll _categoryBLL;
        public BlogBll(IBlogDal dal, ICommentsDal commentsDal, IViewsDal viewsDal, IUserFocusDal focusDal, ICategoryBll categoryBLL)
        {
            _dal = dal;
            _commentsDal = commentsDal;
            _viewsDal = viewsDal;
            _focusDal = focusDal;
            _categoryBLL = categoryBLL;
        }

        public async Task<int> AddBlogAsync(string title, string content,Guid categoryId, Guid userId, bool isAdmin=false)
        {
            return await _dal.AddAsync(new Blog()
            {
                Title = title,
                Content = content,
                CategoryId = categoryId,
                IsAdmin=isAdmin,
                UsersId=userId,
                IsPublic=true
            });
        }
        public async Task<int> AddBlogAsync(string title, string content, Guid categoryId, Guid userId, string Labels,bool isPublic)
        {
            return await _dal.AddAsync(new Blog()
            {
                Title = title,
                Content = content,
                CategoryId = categoryId,
                IsAdmin = false,
                UsersId = userId,
                IsPublic = isPublic,
                Labels=Labels
            });
        }
        public async Task<int> EditBlogAsync(Guid id,string title, Guid categoryId, string content, string Labels, bool isPublic)
        {
            var data = await _dal.QueryAsync(id);
            if (data == null)
                return -1;

            data.Title = title;
            data.Content = content;
            data.UpdateTime = DateTime.Now;
            data.CategoryId = categoryId;
            data.Labels = Labels;
            data.IsPublic = isPublic;
            return await _dal.EditAsync(data);
        }
        public async Task<int> EditBlogPublicAsync(Guid id)
        {
            var data = await _dal.QueryAsync(id);
            if (data == null)
                return -1;

            data.IsPublic = !data.IsPublic;
            return await _dal.EditAsync(data);
        }

        public async Task<int> EditAdminBlogAsync(Guid id, Guid categoryId, bool isPublic)
        {
            var data = await _dal.QueryAsync(id);
            if (data == null)
                return -1;

            data.CategoryId = categoryId;
            data.IsPublic = isPublic;
            data.UpdateTime = DateTime.Now;
            return await _dal.EditAsync(data);
        }

        public async Task<int> DeleteBlogAsync(Guid id)
        {
            
            var data = await _dal.QueryAsync(id);
            if (data == null)
                return -1;
            var commnetList = _commentsDal.Query(x => x.BlogId == id).ToList();
            if (commnetList.Any())
            {
                foreach (var item in commnetList)
                {
                    await _commentsDal.DeleteAsync(item);
                }
            }
            return await _dal.DeleteAsync(data);
        }

        public async Task<List<BlogDto>> GetAllAsync()
        {
            return await _dal.Query()
                .OrderByDescending(c => c.UpdateTime)
                .Select(c => new BlogDto
                {
                    Id = c.Id,
                    Title = c.Title,
                    Content = c.Content,
                    UpdateTime = c.UpdateTime
                }).ToListAsync();
        }
        public async Task<List<BlogDto>> GetFocusListAsync(Guid uid,string search)
        {
            var flowlist = _focusDal.Query(x => x.UserId == uid).ToList() ;
            if(flowlist.Any())
            {
                var uids = flowlist.Select(x => x.BeUserId);
                var query = _dal.Query(x => x.IsPublic);
                query = query.Where(x => uids.Contains(x.UsersId));
                if (!string.IsNullOrWhiteSpace(search))
                {
                    query = query.Where(c => c.Title.Contains(search));
                }
                var list = await query.OrderByDescending(x => x.IsAdmin)
                    .ThenByDescending(c => c.UpdateTime)
                    .Select(c => new BlogDto
                    {
                        Id = c.Id,
                        Title = c.Title,
                        Content = c.Content,
                        UpdateTime = c.UpdateTime,
                        IsPublic = c.IsPublic,
                        CategoryId = c.CategoryId,
                        UsersId = c.UsersId
                    //Comments=_commentsDal.GetCounts(x=>x.BlogId==c.Id),
                    //Views=_viewsDal.GetCounts(x=>x.BlogId==c.Id)
                }).ToListAsync();
                list.ForEach(item =>
                {
                    item.Comments = _commentsDal.GetCounts(x => x.BlogId == item.Id);
                    item.Views = _viewsDal.GetCounts(x => x.BlogId == item.Id);
                });
                return list;
            }

            return await Task.FromResult(new List<BlogDto>());
           
        }
        public async Task<List<BlogCategoryDto>> GetCategroyGroupAsync(Guid uid)
        {
            var query = _dal.Query(x=>x.IsPublic);
            query = query.Where(x => x.UsersId==uid);
             
            var list = await query.GroupBy(x=>x.CategoryId)
                .Select(c => new BlogCategoryDto
                {
                   CategoryId=c.Key,
                   Count=c.Count(),
                   
                }).ToListAsync();
           
            return list;

        }
        public async Task<List<BlogDto>> GetDataByTitleAsync(string title)
        {
            var list =await _dal.Query(c => c.Title.Contains(title)).OrderByDescending(x=>x.UpdateTime)
                //.OrderByDescending(x=>x.IsAdmin)
                //.ThenByDescending(c => c.UpdateTime)
                .Select(c => new BlogDto
                {
                    Id = c.Id,
                    Title = c.Title,
                    Content = c.Content,
                    UpdateTime = c.UpdateTime,
                    IsPublic = c.IsPublic,
                    CategoryId = c.CategoryId,
                    UsersId = c.UsersId
                    //Comments=_commentsDal.GetCounts(x=>x.BlogId==c.Id),
                    //Views=_viewsDal.GetCounts(x=>x.BlogId==c.Id)
                }).ToListAsync();
            list.ForEach(item =>
            {
                item.Comments = _commentsDal.GetCounts(x => x.BlogId == item.Id);
                item.Views = _viewsDal.GetCounts(x => x.BlogId == item.Id);
            });
            return list;
        }
        public async Task<List<BlogDto>> GetMyBlogListAsync(Guid userId,string cid,string title)
        {
            var query = _dal.Query();
            query = query.Where(x => x.UsersId == userId);
            if (!string.IsNullOrWhiteSpace(title))
            {

                query = query.Where(c => c.Title.Contains(title));
            }
            if (!string.IsNullOrWhiteSpace(cid))
            {
                var id = Guid.Parse(cid);

                query = query.Where(c => c.CategoryId==id);
            }
            var list = await query.OrderByDescending(x => x.UpdateTime)
                .Select(c => new BlogDto
                {
                    Id = c.Id,
                    Title = c.Title,
                    Content = c.Content,
                    UpdateTime = c.UpdateTime,
                    IsPublic = c.IsPublic,
                    CategoryId = c.CategoryId,
                    UsersId = c.UsersId
                    //Comments=_commentsDal.GetCounts(x=>x.BlogId==c.Id),
                    //Views=_viewsDal.GetCounts(x=>x.BlogId==c.Id)
                }).ToListAsync();
            list.ForEach(item =>
            {
                item.Comments = _commentsDal.GetCounts(x => x.BlogId == item.Id);
                item.Views = _viewsDal.GetCounts(x => x.BlogId == item.Id);
            });
            return list;
        }
        public async Task<List<BlogDto>> GetDataByCategoryTitleAsync(Guid cid,string title)
        {
            var query = _dal.Query(x => x.IsPublic);
            query = query.Where(x => x.CategoryId == cid);
            if (!string.IsNullOrWhiteSpace(title))
            {

                query = query.Where(c => c.Title.Contains(title));
            }
            var list = await query.OrderByDescending(x => x.IsAdmin)
                .ThenByDescending(c => c.UpdateTime)
                .Select(c => new BlogDto
                {
                    Id = c.Id,
                    Title = c.Title,
                    Content = c.Content,
                    UpdateTime = c.UpdateTime,
                    IsPublic = c.IsPublic,
                    CategoryId = c.CategoryId,
                    UsersId=c.UsersId
                    //Comments=_commentsDal.GetCounts(x=>x.BlogId==c.Id),
                    //Views=_viewsDal.GetCounts(x=>x.BlogId==c.Id)
                }).ToListAsync();
            list.ForEach(item =>
            {
                item.Comments = _commentsDal.GetCounts(x => x.BlogId == item.Id);
                item.Views = _viewsDal.GetCounts(x => x.BlogId == item.Id);
            });
            return list;
        }

        public async Task<BlogDto> GetBlogByIdAsync(Guid id)
        {
            var data = await _dal.QueryAsync(id);
            return data == null ? null : new BlogDto
                {
                    Id = data.Id,
                    Title = data.Title,
                    Content = data.Content,
                    UpdateTime = data.UpdateTime,
                    IsPublic=data.IsPublic,
                    CategoryId=data.CategoryId,
                    UsersId=data.UsersId,
                    Labels=data.Labels
                };
        }

        public async Task<bool> IsExistsAsync(string title)
        {
            var data = _dal.Query(b => b.Title.Equals(title));
            return !(await data.AnyAsync());
        }

        public async Task<List<BlogDto>> GetDataByTop4()
        {
            var category = _categoryBLL.GetNotice1();
            return await _dal.Query(x => x.CategoryId != category.Id&&x.IsPublic)
                .OrderByDescending(b => b.UpdateTime)
                .Select(b => new BlogDto
                {
                    Id = b.Id,
                    Title = b.Title,
                    Content = b.Content,
                    UpdateTime = b.UpdateTime
                }).Take(4).ToListAsync().ConfigureAwait(false);
        }
        public async Task<List<BlogDto>> GetDataByRandom4()
        {
            var category =_categoryBLL.GetNotice1();
            return await _dal.Query(x=>x.CategoryId!=category.Id && x.IsPublic)
                .OrderBy(a=>Guid.NewGuid()).Take(4)
                .Select(b => new BlogDto
                {
                    Id = b.Id,
                    Title = b.Title,
                    Content = b.Content,
                    UpdateTime = b.UpdateTime
                }).ToListAsync().ConfigureAwait(false);
        }

        public async Task<int> GetViewsCount(DateTime start, DateTime end)
        {
            return await _viewsDal.GetCountsAsync(x=>x.CreateTime>=start&&x.CreateTime<=end);
        }

        public async Task<int> GetViewsAllCount()
        {
            return await _viewsDal.GetCountsAsync(x => !x.IsRemoved);
        }
        public async Task Click(Guid id,string ip,string uid)
        {
            var blog = await _dal.QueryAsync(id);
            blog.Views += 1;
            await _dal.EditAsync(blog);
            var viewinfo = new Views
            {
                IP = ip,
                BlogId = id,
                
            };
            if(!string.IsNullOrWhiteSpace(uid))
            {
                viewinfo.UserId = Guid.Parse(uid);
            }
            await _viewsDal.AddAsync(viewinfo);
        }
    }
}