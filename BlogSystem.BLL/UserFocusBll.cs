using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlTypes;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BlogSystem.DAL;
using BlogSystem.Dtos;
using BlogSystem.IBLL;
using BlogSystem.IDAL;
using BlogSystem.Models;

namespace BlogSystem.BLL
{
    public class UserFocusBll : IUserFocusBll
    {
        private IUserFocusDal _dal;
        private IUserMsgDal _msgdal;
        private IUsersDal _usersDal;

        public UserFocusBll(IUserFocusDal dal, IUserMsgDal msgdal, IUsersDal usersDal)
        {
            _dal = dal;
            _msgdal = msgdal;
            _usersDal = usersDal;
        }

        public async Task<int> Focus(Guid userId, Guid beUserId)
        {
            var r= await _dal.AddAsync(new UserFocus
            {
                UserId = userId,
                BeUserId = beUserId,

            });
            var u = await _usersDal.QueryAsync(userId);
           await _msgdal.AddAsync(new UserMsg
            {
                UserId = beUserId,
                Contents = $"{u.Email}关注了你"
            });
            return r;
        }

        public async Task<int> GetBeFocusCount(Guid userId)
        {
            return await _dal.GetCountsAsync(x => x.BeUserId == userId);
        }

        public async Task<int> GetFocusCount(Guid userId)
        {
            return await _dal.GetCountsAsync(x => x.UserId == userId);
        }

        public async Task<bool> IsFocus(Guid userId, Guid beUserId)
        {
            return await _dal.IsExistsAsync(x => x.UserId == userId && x.BeUserId == beUserId);
        }
    }
}