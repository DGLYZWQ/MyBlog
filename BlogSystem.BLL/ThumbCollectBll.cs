using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using BlogSystem.Dtos;
using BlogSystem.IBLL;
using BlogSystem.IDAL;
using BlogSystem.Models;


namespace BlogSystem.BLL
{
    public class ThumbCollectBll: IThumbCollectBll
    {
        private readonly IThumbCollectDal _thumbCollectDal;
        private readonly IBlogDal _blogDal;
        public ThumbCollectBll(IThumbCollectDal thumbCollectDal,IBlogDal blogDal)
        {
            _thumbCollectDal = thumbCollectDal;
            _blogDal = blogDal;
        }

        public async Task<int> AddAsync(ThumbCollectDto dto)
        {
            var entity =  _thumbCollectDal.Query(x => x.UsersId == dto.UsersId&&x.BlogId==dto.BlogId && x.OptType == dto.OptType).FirstOrDefault();
            if(entity==null)
            {
                return await _thumbCollectDal.AddAsync(new ThumbCollect
                {
                    BlogId = dto.BlogId,
                    UsersId = dto.UsersId,
                    OptType = dto.OptType,

                });
            }
            else
            {
                return await _thumbCollectDal.DeleteAsync(entity);
            }
        }
        public async Task<int> GetThumpCount(Guid blogId)
        {
            return await _thumbCollectDal.GetCountsAsync(x => x.BlogId == blogId&&x.OptType==1);
        }
        public async Task<List<ThumbCollectBlogDto>> GetList(Guid userId, int optType)
        {
            var list = _thumbCollectDal.Query(x => x.UsersId == userId && x.OptType == optType).ToList();
            if(list.Any())
            {
                var blogList = from t in list
                               join c in _blogDal.Query().ToList() on t.BlogId equals c.Id
                               select new ThumbCollectBlogDto
                               {
                                   Id = c.Id,
                                   Title = c.Title,
                                   Content = c.Content,
                                   UpdateTime = c.UpdateTime,
                                   IsPublic = c.IsPublic,
                                   CategoryId = c.CategoryId,
                                   UsersId = c.UsersId,
                                   ThumbCollectId = t.Id,
                                   ThumbCollectTime = t.CreateTime,
                                   Labels = c.Labels
                               };
                return await Task.FromResult(blogList.ToList());
            }
            return await Task.FromResult(new List<ThumbCollectBlogDto>());
        }

        public async Task<bool> IsExits(Guid userId, Guid blogId, int optType)
        {
            var entity= await _thumbCollectDal.Query(x => x.UsersId == userId && x.BlogId == blogId && x.OptType == optType).FirstOrDefaultAsync();
            return entity != null;
        }
    }
}
