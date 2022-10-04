using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlogSystem.Dtos
{
    public interface ICommentsBll
    {
        Task<int> AddCommentAsync(string content);
        Task<int> EditCommentAsync(Guid id,string content);
        Task<int> DeleteCommentAsync(Guid id);
        Task<List<CommentsDto>> GetAllAsync();
        Task<CommentsDto> GetDataByBlogIdAsync(Guid blogId);
        Task<CommentsDto> GetDataByUsersIdAsync(Guid userId);
        Task<bool> IsExistsAsync(string content);
        

    }
}