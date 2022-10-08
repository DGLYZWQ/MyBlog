using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlTypes;
using System.Linq;
using System.Threading.Tasks;
using BlogSystem.Dtos;
using BlogSystem.IBLL;
using BlogSystem.IDAL;
using BlogSystem.Models;

namespace BlogSystem.BLL
{
    public class UsersBll : IUsersBll
    {
        private IUsersDal _dal;

        public UsersBll(IUsersDal dal)
        {
            _dal = dal;
        }

        public async Task<int> AddUsersAsync(string email, string password, string nickname, string avatar, string image, Guid rolesId)
        {
            return await _dal.AddAsync(new Users()
            {
                Email = email,
                Password = password,
                NickName = nickname,
                Avatar = avatar,
                Image = image,
                RolesId = rolesId
            });
        }


        public async Task<int> EditUsersAsync(Guid id, string password, string nickname, string avatar, string image, Guid rolesId)
        {
            var data = await _dal.QueryAsync(id);
            if (data == null)
                return -1;
            if (image != null)
                data.Image = image;
            data.NickName = nickname;
            data.Password = password;
            if (avatar != null)
                data.Avatar = avatar;
            data.RolesId = rolesId;
            return await _dal.EditAsync(data);
        }

        public async Task<int> DeleteUsersAsync(Guid id)
        {
            var data = await _dal.QueryAsync(id);
            if (data != null)
                return await _dal.DeleteAsync(data);
            return -2;
        }

        public async Task<UsersDto> LoginAsync(string email, string password)
        {
            return await _dal.Query(user => user.Email.Equals(email) && user.Password.Equals(password))
                .Select(u => new UsersDto()
                {
                    Id = u.Id,
                    Email = u.Email,
                    Password = u.Password,
                    NickName = u.NickName,
                    RolesId  = u.RolesId,
                    Avatar = u.Avatar,
                    Image = u.Image,
                    UpdateTime = u.UpdateTime
                }).FirstOrDefaultAsync();
        }

        public async Task<List<UsersDto>> GetAllUsersAsync()
        {
            return await _dal.Query().Select(u => new UsersDto()
            {
                Id = u.Id,
                Email = u.Email,
                Password = u.Password,
                NickName = u.NickName,
                RolesId = u.RolesId,
                Avatar = u.Avatar,
                Image = u.Image,
                UpdateTime = u.UpdateTime
            }).ToListAsync();
        }

        public async Task<List<UsersDto>> GetUsersByNickName(string nickname)
        {
            return await _dal.Query(u => u.NickName.Contains(nickname))
                .OrderByDescending(u => u.UpdateTime)
                .Select(u => new UsersDto()
                {
                    Id = u.Id,
                    Email = u.Email,
                    Password = u.Password,
                    NickName = u.NickName,
                    RolesId = u.RolesId,
                    Avatar = u.Avatar,
                    Image = u.Image,
                    UpdateTime = u.UpdateTime
                }).ToListAsync();
        }

        public async Task<bool> IsExists(string email)
        {
            return await _dal.IsExistsAsync(ur => ur.Email.Equals(email));
        }

        public async Task<UsersDto> GetUsersByEmail(string email)
        {
            return await _dal.Query(u => u.Email.Equals(email)).Select(u => new UsersDto()
                {
                   Id = u.Id,
                   Email = u.Email,
                   Password = u.Password,
                   NickName = u.NickName,
                   RolesId = u.RolesId,
                   Avatar = u.Avatar,
                   Image = u.Image,
                   UpdateTime = u.UpdateTime
                }).FirstOrDefaultAsync();
        }

        public async Task<UsersDto> GetUsersById(Guid id)
        {
            var data = await _dal.QueryAsync(id);
            if (data != null)
            {
                return new UsersDto()
                {
                    Id = data.Id,
                    Email = data.Email,
                    Image = data.Image,
                    NickName = data.NickName,
                    Password = data.Password,
                    Avatar = data.Avatar,
                    RolesId = data.RolesId,
                    UpdateTime = data.UpdateTime
                };
            }

            return null;
        }

        public async Task<List<UsersDto>> GetUsersByRolesId(Guid rid)
        {
            return await _dal.Query(u => u.RolesId == rid).Select(u => new UsersDto()
            {
                Id = u.Id,
                Email = u.Email,
                Password = u.Password,
                NickName = u.NickName,
                RolesId = u.RolesId,
                Avatar = u.Avatar,
                Image = u.Image,
                UpdateTime = u.UpdateTime
            }).ToListAsync();
        }
    }
}