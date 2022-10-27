using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlogSystem.Dtos
{
    public interface ICommentsBll
    {
        Task<int> AddCommentAsync(Guid blogId, Guid userId, string content);
        Task<int> EditCommentAsync(Guid id,string content);
        Task<int> DeleteCommentAsync(Guid id);
        Task<List<CommentsDto>> GetAllAsync();
        Task<List<CommentsDto>> GetAllAsync(Guid blogId);
        Task<CommentsDto> GetDataByBlogIdAsync(Guid blogId);
        Task<CommentsDto> GetDataByUsersIdAsync(Guid userId);
        Task<bool> IsExistsAsync(string content);
        Task<int> Check(Guid id, bool isCheck);
        Task<int> GetCount(DateTime start, DateTime end);
        Task<int> GetAllCount();
        Task<int> GetCount(Guid blogId);


    }
}