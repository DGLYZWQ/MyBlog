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
        private IUserMsgDal _msgdal;
        private IUsersDal _usersDal;
        private IBlogDal _blogDal;
        public CommentsBll(ICommentsDal dal, IUserMsgDal msgdal, IUsersDal usersDal, IBlogDal blogDal)
        {
            _dal = dal;
            _msgdal = msgdal;
            _usersDal = usersDal;
            _blogDal = blogDal;
        }

        public async Task<int> AddCommentAsync(Guid blogId, Guid userId, string content)
        {
            var u = await _usersDal.QueryAsync(userId);
            var blog = await _blogDal.QueryAsync(blogId);
            
            await _msgdal.AddAsync(new UserMsg
            {
                UserId = blog.UsersId,
                Contents = $"{u.Email}评论了你的博客《{blog.Title}》"
            });
            return await _dal.AddAsync(new Comments()
           {
               Content = content,
               BlogId=blogId,
               UserId=userId
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
                    UpdateTime = c.UpdateTime,
                    IsChecked=c.IsChecked
                }).ToListAsync();
        }
        public async Task<List<CommentsDto>> GetAllAsync(Guid blogId)
        {
            return await _dal.Query(x=>x.BlogId==blogId)
                .OrderByDescending(c => c.UpdateTime)
                .Select(c => new CommentsDto()
                {
                    Id = c.Id,
                    BlogId = c.BlogId,
                    Content = c.Content,
                    UpdateTime = c.UpdateTime,
                    IsChecked = c.IsChecked,
                    UserId=c.UserId
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
        public async Task<int> Check(Guid id,bool isCheck)
        {
            var data = await _dal.QueryAsync(id);
            if (data == null)
                return -1;
            data.IsChecked = isCheck;
            data.UpdateTime = DateTime.Now;
            return await _dal.EditAsync(data);
        }
        public async Task<int> GetCount(DateTime start, DateTime end)
        {
            return await _dal.GetCountsAsync(x => x.CreateTime >= start && x.CreateTime <= end);
        }

        public async Task<int> GetAllCount()
        {
            return await _dal.GetCountsAsync(x => !x.IsRemoved);
        }
        public async Task<int> GetCount(Guid blogId)
        {
            return await _dal.GetCountsAsync(x => x.BlogId==blogId);
        }
    }
}