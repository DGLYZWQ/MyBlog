using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BlogSystem.Dtos;

namespace BlogSystem.IBLL
{
    public interface IUserMsgBll
    {
        Task<List<UserMsgDto>> GetList(Guid userId);

    }
}