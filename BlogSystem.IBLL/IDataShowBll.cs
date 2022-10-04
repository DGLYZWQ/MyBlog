using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BlogSystem.Dtos;

namespace BlogSystem.IBLL
{
    public interface IDataShowBll
    {
        Task<int> AddDataShowAsync(string title, string icons);
        Task<int> EditDataSHowAsync(Guid id,string title, string icons);
        Task<int> DeleteDataShowAsync(Guid id);
        Task<List<DataShowDto>> GetAllAsync();
    }
}