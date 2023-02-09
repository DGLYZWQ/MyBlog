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
        public async Task<int> CancelFocus(Guid userId, Guid beUserId)
        {
            
            var entity = _dal.Query(x => x.UserId == userId && x.BeUserId == beUserId).FirstOrDefault();
            if(entity!=null)
            {
               var r= await _dal.DeleteAsync(entity);

                var u = await _usersDal.QueryAsync(userId);
                await _msgdal.AddAsync(new UserMsg
                {
                    UserId = beUserId,
                    Contents = $"{u.Email}取消关注了你"
                });
                return r;
            }
            return -1;
        }

        public async Task<int> GetBeFocusCount(Guid userId)
        {
            return await _dal.GetCountsAsync(x => x.BeUserId == userId);
        }

        public async Task<int> GetFocusCount(Guid userId)
        {
            return await _dal.GetCountsAsync(x => x.UserId == userId);
        }
        public async Task<List<UsersDto>> GetFocusList(Guid userId)
        {
            var list = _dal.Query(x => x.UserId == userId);
            if(list.Any())
            {
                var userList = from t in list.ToList()
                               join u in _usersDal.Query().ToList() on t.BeUserId equals u.Id
                               select new UsersDto
                               {
                                   Id = u.Id,
                                   NickName = u.NickName,
                                   Image = u.Image,
                                   Intro = u.Intro
                               };
                return await Task.FromResult(userList.ToList());
            }
            return await Task.FromResult(new List<UsersDto>());

        }
        public async Task<bool> IsFocus(Guid userId, Guid beUserId)
        {
            return await _dal.IsExistsAsync(x => x.UserId == userId && x.BeUserId == beUserId);
        }
    }
}