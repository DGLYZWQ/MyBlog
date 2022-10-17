using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using BlogSystem.Dtos;
using BlogSystem.IDAL;
using BlogSystem.Models;

namespace BlogSystem.BLL
{
    public class BlogBll : IBlogBll
    {
        private IBlogDal _dal;

        public BlogBll(IBlogDal dal)
        {
            _dal = dal;
        }

        public async Task<int> AddBlogAsync(string title, string content,Guid categoryId)
        {
            return await _dal.AddAsync(new Blog()
            {
                Title = title,
                Content = content,
                CategoryId = categoryId
            });
        }

        public async Task<int> EditBlogAsync(Guid id,string title, string content)
        {
            var data = await _dal.QueryAsync(id);
            if (data == null)
                return -1;

            data.Title = title;
            data.Content = content;
            data.UpdateTime = DateTime.Now;
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

        public async Task<List<BlogDto>> GetDataByTitleAsync(string title)
        {
            return await _dal.Query(c => c.Title.Contains(title))
                .OrderByDescending(c => c.UpdateTime)
                .Select(c => new BlogDto
                {
                    Id = c.Id,
                    Title = c.Title,
                    Content = c.Content,
                    UpdateTime = c.UpdateTime
                }).ToListAsync();
        }

        public async Task<BlogDto> GetBlogByIdAsync(Guid id)
        {
            var data = await _dal.QueryAsync(id);
            return data == null ? null : new BlogDto
                {
                    Id = data.Id,
                    Title = data.Title,
                    Content = data.Content,
                    UpdateTime = data.UpdateTime
                };
        }

        public async Task<bool> IsExistsAsync(string title)
        {
            var data = _dal.Query(b => b.Title.Equals(title));
            return !(await data.AnyAsync());
        }

        public async Task<List<BlogDto>> GetDataByTop4()
        {
            return await _dal.Query()
                .OrderByDescending(b => b.UpdateTime)
                .Select(b => new BlogDto
                {
                    Id = b.Id,
                    Title = b.Title,
                    Content = b.Content,
                    UpdateTime = b.UpdateTime
                }).Take(4).ToListAsync();
        }
    }
}