using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlogSystem.Dtos
{
    public interface IBlogBll
    {
        Task<int> AddBlogAsync(string title, string content);
        Task<int> EditBlogAsync(Guid id, string title, string content);
        Task<int> DeleteBlogAsync(Guid id);
        Task<List<BlogDto>> GetAllAsync();
        Task<List<BlogDto>> GetDataByTitleAsync(string title);
        Task<BlogDto> GetBlogByIdAsync(Guid id);
        Task<bool> IsExistsAsync(string title);
        Task<List<BlogDto>> GetDataByTop4();
    }
}