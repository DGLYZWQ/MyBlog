using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BlogSystem.Dtos;

namespace BlogSystem.IBLL
{
    public interface ICategoryBll
    {
        Task<int> AddCategoryAsync(string title, string description);
        Task<int> EditCategoryAsync(Guid id,string title, string description);
        Task<int> DeleteCategoryAsync(Guid id);
        Task<List<CategoryDto>> GetAllAsync();
        Task<List<CategoryDto>> GetDataByTitleAsync(string title);
        Task<CategoryDto> GetCategoryByIdAsync(Guid id);
        Task<bool> IsExistsAsync(string title);
        Task<List<CategoryDto>> GetDataByTop4();
    }
}