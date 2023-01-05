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
        private IBlogDal _blogDal;
        private ICategoryDal _categoryDal;
        private IUsersBll _usersDal;

        public UserMsgBll(IUserMsgDal dal, IBlogDal blogDal, ICategoryDal categoryDal, IUsersBll usersDal)
        {
            _dal = dal;
            _blogDal = blogDal;
            _categoryDal = categoryDal;
            _usersDal = usersDal;
        }

        public async Task<List<UserMsgDto>> GetList(Guid userId)
        {
            var list = await _dal.Query(x => x.UserId == userId).OrderByDescending(x => x.CreateTime)
               .Select(x => new UserMsgDto
               {
                   Contents = x.Contents,
                   CreateTime = x.CreateTime
               }).ToListAsync();
            var category = _categoryDal.Query(x => x.Title == "公告").FirstOrDefault();
            if(category!=null)
            {
                var noticeList = _blogDal.Query(x => x.CategoryId==category.Id);
                foreach(var item in noticeList)
                {
                    var user = await _usersDal.GetUsersById(item.UsersId);
                    list.Add(new UserMsgDto
                    {
                        Contents = $"{user?.Email}发布了公告：《{item.Title}》",
                        CreateTime=item.UpdateTime,
                        Notice=item.Content
                        
                    });
                }

            }
            list=list.OrderByDescending(x => x.CreateTime).ToList();
            return list;
        }
    }
}