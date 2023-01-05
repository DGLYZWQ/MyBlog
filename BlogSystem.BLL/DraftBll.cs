using BlogSystem.Dtos;
using BlogSystem.IBLL;
using BlogSystem.IDAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.BLL
{
    public class DraftBll : IDraftBll
    {
        private readonly IDraftDal _draftDal;
        public DraftBll(IDraftDal draftDal)
        {
            _draftDal = draftDal;
        }
        public async Task<DraftDto> GetTop1Async(Guid userid)
        {
            return await _draftDal.Query(x => x.UsersId == userid).OrderByDescending(x => x.CreateTime).Take(1)
                .Select(x=>new DraftDto
                {
                    CreateTime=x.CreateTime,
                    CategoryId=x.CategoryId,
                    Content=x.Content,
                    Id=x.Id,
                    IsPublic=x.IsPublic,
                    IsRemoved=x.IsRemoved,
                    Labels=x.Labels,
                    Title=x.Title,
                    UpdateTime=x.UpdateTime,
                    UsersId=x.UsersId
                })
                .FirstOrDefaultAsync();
        }
        public async Task<List<DraftDto>> GetList(Guid userid)
        {
            return await _draftDal.Query(x => x.UsersId == userid).OrderByDescending(x => x.CreateTime)
                .Select(x => new DraftDto
                {
                    CreateTime = x.CreateTime,
                    CategoryId = x.CategoryId,
                    Content = x.Content,
                    Id = x.Id,
                    IsPublic = x.IsPublic,
                    IsRemoved = x.IsRemoved,
                    Labels = x.Labels,
                    Title = x.Title,
                    UpdateTime = x.UpdateTime,
                    UsersId = x.UsersId
                }).ToListAsync();
        }
        public async Task<int> AddAsync(DraftDto dto)
        {
            var entity =await GetTop1Async(dto.UsersId);
            if(entity==null)
            return await _draftDal.AddAsync(new Models.Draft
            {
                CategoryId = dto.CategoryId,
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now,
                Content = dto.Content,
                IsPublic = dto.IsPublic,
                IsRemoved = false,
                Labels = dto.Labels,
                Title = dto.Title,
                UsersId = dto.UsersId
            });
            else
            {
                var model = await _draftDal.QueryAsync(entity.Id);
                model.CategoryId = dto.CategoryId;
                model.CreateTime = DateTime.Now;
                model.UpdateTime = DateTime.Now;
                model.Content = dto.Content;
                model.IsPublic = dto.IsPublic;
                model.IsRemoved = dto.IsRemoved;
                model.Labels = dto.Labels;
                model.Title = dto.Title;
                model.UsersId = dto.UsersId;
                return await _draftDal.EditAsync(model);
            }
        }

        public async Task<int> Delete(Guid id)
        {
            var entity = await _draftDal.QueryAsync(id);
            return await _draftDal.DeleteAsync(entity);
        }
    }
}
