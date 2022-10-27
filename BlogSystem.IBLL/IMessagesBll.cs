using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlogSystem.Dtos
{
    public interface IMessagesBll
    {
        Task<int> AddMessageAsync(string name, string email, string tel, string content);
        Task<int> EditMessageAsync(Guid id,string name, string email, string tel, string content);
        Task<int> DeleteMessageAsync(Guid id);
        Task<List<MessagesDto>> GetAllAsync();
        Task<List<MessagesDto>> GetDataByNameAsync(string name);
        Task<MessagesDto> GetMessagesByIdAsync(Guid id);
        Task<int> Read(Guid id, bool isRead);
        int GetNoReadCounts();
        Task<int> GetViewsCount(DateTime start, DateTime end);
        Task<int> GetViewsAllCount();

    }
}