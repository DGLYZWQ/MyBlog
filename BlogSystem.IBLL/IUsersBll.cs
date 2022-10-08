using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BlogSystem.Dtos;

namespace BlogSystem.IBLL
{
    public interface IUsersBll
    {
        Task<int> AddUsersAsync(string email, string password, string nickname, string avatar, string image,
            Guid rolesId);

        Task<int> EditUsersAsync(Guid id, string password, string nickname, string avatar, string image,
            Guid rolesId);

        Task<int> DeleteUsersAsync(Guid id);
        Task<UsersDto> LoginAsync(string email, string password);
        Task<List<UsersDto>> GetAllUsersAsync();
        Task<List<UsersDto>> GetUsersByNickName(string nickname);
        Task<bool> IsExists(string email);
        Task<UsersDto> GetUsersByEmail(string email);
        Task<UsersDto> GetUsersById(Guid id);
        Task<List<UsersDto>> GetUsersByRolesId(Guid rid);

    }
}