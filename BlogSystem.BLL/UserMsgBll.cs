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
    public class UserMsgBll : IUserMsgBll
    {
        private IUserMsgDal _dal;

        public UserMsgBll(IUserMsgDal dal)
        {
            _dal = dal;
        }

        public async Task<List<UserMsgDto>> GetList(Guid userId)
        {
            return await _dal.Query(x => x.UserId == userId).OrderByDescending(x => x.CreateTime)
                .Select(x => new UserMsgDto
                {
                    Contents=x.Contents,
                    CreateTime=x.CreateTime
                }).ToListAsync();
        }
    }
}