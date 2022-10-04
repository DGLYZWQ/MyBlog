using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using BlogSystem.Dtos;
using BlogSystem.IDAL;
using BlogSystem.Models;

namespace BlogSystem.BLL
{
    public class CommentsBll : ICommentsBll
    {
        private ICommentsDal _dal;

        public CommentsBll(ICommentsDal dal)
        {
            _dal = dal;
        }

        public async Task<int> AddCommentAsync(string content)
        {
           return await _dal.AddAsync(new Comments()
           {
               Content = content
           });
        }

        public async Task<int> EditCommentAsync(Guid id,string content)
        {
            var data = await _dal.QueryAsync(id);
            if (data == null)
                return -1;
            data.Content = content;
            data.UpdateTime = DateTime.Now;
            return await _dal.EditAsync(data);
        }

        public async Task<int> DeleteCommentAsync(Guid id)
        {
            var data = await _dal.QueryAsync(id);
            if (data == null)
                return -1;
            return await _dal.DeleteAsync(data);
        }

        public async Task<List<CommentsDto>> GetAllAsync()
        {
            return await _dal.Query()
                .OrderByDescending(c => c.UpdateTime)
                .Select(c => new CommentsDto()
                {
                    Id = c.Id,
                    BlogId = c.BlogId,
                    Content = c.Content,
                    UpdateTime = c.UpdateTime
                }).ToListAsync();
        }

        public async Task<CommentsDto> GetDataByBlogIdAsync(Guid blogId)
        {
            var c = await _dal.QueryAsync(blogId);
            return c == null
                ? null
                : new CommentsDto
                {
                    Id = c.Id,
                    BlogId = c.BlogId,
                    Content = c.Content,
                    UpdateTime = c.UpdateTime
                };
        }

        public async Task<CommentsDto> GetDataByUsersIdAsync(Guid userId)
        {
            var data = await _dal.QueryAsync(userId);
            return data == null
                ? null
                : new CommentsDto
                {
                    Id = data.Id,
                    BlogId = data.BlogId,
                    Content = data.Content,
                    UpdateTime = data.UpdateTime
                };
        }

        public async Task<bool> IsExistsAsync(string content)
        {
            var data = _dal.Query(c => c.Content.Contains(content));
            return !(await data.AnyAsync());
        }
    }
}