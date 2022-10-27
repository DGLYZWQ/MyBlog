using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using BlogSystem.Dtos;
using BlogSystem.IDAL;
using BlogSystem.Models;

namespace BlogSystem.BLL
{
    public class MessagesBll : IMessagesBll
    {
        private IMessagesDal _dal;

        public MessagesBll(IMessagesDal dal)
        {
            _dal = dal;
        }

        public async Task<int> AddMessageAsync(string name, string email, string tel, string content)
        {
            return await _dal.AddAsync(new Messages()
            {
                Name = name,
                Email = email,
                Content = content,
                Tel = tel
            });
        }

        public async Task<int> EditMessageAsync(Guid id,string name, string email, string tel, string content)
        {
            var data = await _dal.QueryAsync(id);
            if (data == null)
                return -1;
            data.Name = name;
            data.Email = email;
            data.Tel = tel;
            data.Content = content;
            data.UpdateTime = DateTime.Now;
            return await _dal.EditAsync(data);
        }

        public async Task<int> DeleteMessageAsync(Guid id)
        {
            var data = await _dal.QueryAsync(id);
            if (data == null)
                return -1;
            return await _dal.DeleteAsync(data);
        }

        public async Task<List<MessagesDto>> GetAllAsync()
        {
            return await _dal.Query()
                .OrderByDescending(c => c.UpdateTime)
                .Select(c => new MessagesDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Email = c.Email,
                    Tel = c.Tel,
                    Content = c.Content,
                    UpdateTime = c.UpdateTime,
                    IsRead=c.IsRead
                }).ToListAsync();
        }

        public async Task<List<MessagesDto>> GetDataByNameAsync(string name)
        {
            return await _dal.Query(m => m.Name.Contains(name))
                .OrderByDescending(m => m.UpdateTime)
                .Select(m => new MessagesDto
                {
                    Id = m.Id,
                    Name = m.Name,
                    Email = m.Email,
                    Tel = m.Tel,
                    Content = m.Content,
                    UpdateTime = m.UpdateTime
                }).ToListAsync();
        }

        public async Task<MessagesDto> GetMessagesByIdAsync(Guid id)
        {
            var data = await _dal.QueryAsync(id);
            return data == null
                ? null
                : new MessagesDto
                {
                    Id = data.Id,
                    Name = data.Name,
                    Email = data.Email,
                    Tel = data.Tel,
                    Content = data.Content,
                    UpdateTime = data.UpdateTime
                };
        }
        public async Task<int> Read(Guid id, bool isRead)
        {
            var data = await _dal.QueryAsync(id);
            if (data == null)
                return -1;
            data.IsRead = isRead;
            data.UpdateTime = DateTime.Now;
            return await _dal.EditAsync(data);
        }

        public int GetNoReadCounts()
        {
            return _dal.GetCounts(x => !x.IsRead);
        }
        public async Task<int> GetViewsCount(DateTime start, DateTime end)
        {
            return await _dal.GetCountsAsync(x => x.CreateTime >= start && x.CreateTime <= end);
        }

        public async Task<int> GetViewsAllCount()
        {
            return await _dal.GetCountsAsync(x => !x.IsRemoved);
        }
    }
}