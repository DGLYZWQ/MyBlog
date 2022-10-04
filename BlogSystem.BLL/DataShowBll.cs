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
    public class DataShowBll : IDataShowBll
    {
        private IDataShowDal _dal;

        public DataShowBll(IDataShowDal dal)
        {
            _dal = dal;
        }

        public async Task<int> AddDataShowAsync(string title, string icons)
        {
            return await _dal.AddAsync(new DataShow()
            {
                Title = title,
                Icons = icons
            });
        }

        public async Task<int> EditDataSHowAsync(Guid id, string title, string icons)
        {
            var data = await _dal.QueryAsync(id);
            if (data == null)
                return -1;
            data.Title = title;
            data.Icons = icons;
            data.UpdateTime = DateTime.Now;
            return await _dal.EditAsync(data);
        }

        public async Task<int> DeleteDataShowAsync(Guid id)
        {
            var data = await _dal.QueryAsync(id);
            if (data == null)
                return -1;
            return await _dal.DeleteAsync(data);
        }

        public async Task<List<DataShowDto>> GetAllAsync()
        {
            return await _dal.Query()
                .OrderByDescending(d => d.UpdateTime)
                .Select(d => new DataShowDto
                {
                    Id = d.Id,
                    Title = d.Title,
                    Icons = d.Icons,
                    UpdateTime = d.UpdateTime
                }).ToListAsync();
        }
    }
}