using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using BlogSystem.Dtos;
using BlogSystem.IBLL;
using BlogSystem.IDAL;
using BlogSystem.Models;

namespace BlogSystem.BLL
{
    public class CategoryBll : ICategoryBll
    {
        private ICategoryDal _dal;

        public CategoryBll(ICategoryDal dal)
        {
            _dal = dal;
        }

        public async Task<int> AddCategoryAsync(string title, string description)
        {
            return await _dal.AddAsync(new Category()
            {
                Title = title,
                Description = description
            });
        }

        public async Task<int> EditCategoryAsync(Guid id,string title, string description)
        {
            var data = await _dal.QueryAsync(id);
            if (data == null)
                return -1;

            data.Title = title;
            data.Description = description;
            data.UpdateTime = DateTime.Now;
            return await _dal.EditAsync(data);
        }

        public async Task<int> DeleteCategoryAsync(Guid id)
        {
            var data = await _dal.QueryAsync(id);
            if (data == null)
                return -1;
            return await _dal.DeleteAsync(data);
        }

        public async Task<List<CategoryDto>> GetAllAsync()
        {
            return await _dal.Query()
                .OrderByDescending(c => c.UpdateTime)
                .Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Title = c.Title,
                    Description = c.Description,
                    UpdateTime = c.UpdateTime
                }).ToListAsync();
        }

        public async Task<List<CategoryDto>> GetDataByTitleAsync(string title)
        {
            return await _dal.Query(c => c.Title.Contains(title))
                .OrderByDescending(c => c.UpdateTime)
                .Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Title = c.Title,
                    Description = c.Description,
                    UpdateTime = c.UpdateTime
                }).ToListAsync();
        }

        public async Task<CategoryDto> GetCategoryByIdAsync(Guid id)
        {
            var data = await _dal.QueryAsync(id);

            return data == null ? null : new CategoryDto
            {
                Id = data.Id,
                Title = data.Title,
                Description = data.Description,
                UpdateTime = data.UpdateTime
            };
        }

        public async Task<bool> IsExistsAsync(string title)
        {
            var data = _dal.Query(c => c.Title.Equals(title));
            return !(await data.AnyAsync()); //找到元素时，返回false，告诉前面这个类别已存在
        }

        public async Task<List<CategoryDto>> GetDataByTop4()
        {
            return await _dal.Query()
                .OrderByDescending(c => c.UpdateTime)
                .Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Title = c.Title,
                    Description = c.Description,
                    UpdateTime = c.UpdateTime
                }).Take(4).ToListAsync();
        }
    }
}