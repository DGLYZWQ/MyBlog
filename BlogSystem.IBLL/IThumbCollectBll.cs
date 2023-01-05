using BlogSystem.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.IBLL
{
    public interface IThumbCollectBll
    {
        Task<bool> IsExits(Guid userId, Guid blogId, int optType);
        Task<int> AddAsync(ThumbCollectDto dto);
        Task<List<ThumbCollectBlogDto>> GetList(Guid userId, int optType);
        Task<int> GetThumpCount(Guid blogId);
    }
}
