using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BlogSystem.Dtos;

namespace BlogSystem.IBLL
{
    public interface IUserFocusBll
    {

        Task<int> GetFocusCount(Guid userId);
        Task<int> GetBeFocusCount(Guid userId);
        Task<bool> IsFocus(Guid userId, Guid beUserId);

        Task<int> Focus(Guid userId, Guid beUserId);

    }
}