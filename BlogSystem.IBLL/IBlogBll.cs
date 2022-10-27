using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BlogSystem.Dtos
{
    public interface IBlogBll
    {
        Task<int> AddBlogAsync(string title, string content, Guid categoryId, Guid userId, bool isAdmin = false);
        Task<int> EditBlogAsync(Guid id, string title, Guid categoryId, string content);
        Task<int> EditAdminBlogAsync(Guid id, Guid categoryId, bool isPublic);
        Task<int> DeleteBlogAsync(Guid id);
        Task<List<BlogDto>> GetAllAsync();
        Task<List<BlogCategoryDto>> GetCategroyGroupAsync(Guid uid);
        Task<List<BlogDto>> GetFocusListAsync(Guid uid, string search);
        Task<List<BlogDto>> GetDataByTitleAsync(string title);
        Task<List<BlogDto>> GetDataByCategoryTitleAsync(Guid cid, string title);
        Task<List<BlogDto>> GetMyBlogListAsync(Guid userId,string cid, string title);
        Task<BlogDto> GetBlogByIdAsync(Guid id);
        Task<bool> IsExistsAsync(string title);
        Task<List<BlogDto>> GetDataByTop4();
        Task<List<BlogDto>> GetDataByRandom4();
        Task<int> GetViewsCount(DateTime start,DateTime end);
        Task<int> GetViewsAllCount();
        Task Click(Guid id, string ip, string uid);
    }
}