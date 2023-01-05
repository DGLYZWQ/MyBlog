using BlogSystem.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.IBLL
{
    public interface IDraftBll
    {
        Task<int> AddAsync(DraftDto dto);
        Task<DraftDto> GetTop1Async(Guid userid);
        Task<List<DraftDto>> GetList(Guid userid);
        Task<int> Delete(Guid id);
    }
}
