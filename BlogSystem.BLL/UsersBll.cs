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
    public class UsersBll : IUsersBll
    {
        private IUsersDal _dal;
        private IBlogBll _blogBll;

        public UsersBll(IUsersDal dal, IBlogBll blogBll)
        {
            _dal = dal;
            _blogBll = blogBll;
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


        public async Task<int> EditUsersAsync(Guid id, string email, string password, string nickname, string avatar, string image, Guid rolesId)
        {
            //return await _dal.EditAsync(new Users()
            //{
            //    Id = id,
            //    Email = email,
            //    Password = password,
            //    NickName = nickname,
            //    Avatar = avatar,
            //    Image = image,
            //    RolesId = rolesId
            //});
            var data = await _dal.QueryAsync(id);
            if (data == null)
                return -1;
            data.Email = email;
            if (image != null)
                data.Image = image;
            data.NickName = nickname;
            data.Password = password;
            if (avatar != null)
                data.Avatar = avatar;
            data.RolesId = rolesId;
            return await _dal.EditAsync(data);
        }
        public async Task<int> UpdateInfo(Guid id, string email, string password, string nickname, string avatar, string image,string intro)
        {
            
            var data = await _dal.QueryAsync(id);
            if (data == null)
                return -1;
            data.Email = email;
            if (image != null)
                data.Image = image;
            data.NickName = nickname;
            data.Password = password;
            if (avatar != null)
                data.Avatar = avatar;
            if(!string.IsNullOrWhiteSpace(intro))
            data.Intro = intro;
            return await _dal.EditAsync(data);
        }

        public async Task<int> DeleteUsersAsync(Guid id)
        {
            var data = await _dal.QueryAsync(id);
            if (data != null)
            {
                var blogList =await _blogBll.GetMyBlogListAsync(data.Id, "", "");
                if(blogList.Any())
                {
                    foreach(var item in blogList)
                    {
                        await _blogBll.DeleteBlogAsync(item.Id);
                    }
                }
                return await _dal.DeleteAsync(data);
            }
                
            return -2;
        }

        public async Task RegisterAsync(string email, string password,Guid rolesId, string nickName= "Admin")
        {
              await _dal.AddAsync( new Users()
                {
                    Email = email,
                    Password = password,
                    NickName = nickName,
                    Avatar = "defaultbg.jpg",
                    Image = "default.jpg",
                    RolesId = rolesId
              });
        }
        public async Task<int> ResetPwd(string email, string password)
        {
            var info =await _dal.Query(u => u.Email == email).FirstAsync();
            info.Password = password;
            return await _dal.EditAsync(info);
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
        public UsersDto FindOne(string uuid, string fromto)
        {
            return _dal.Query(user => user.uuid.Equals(uuid) && user.fromto.Equals(fromto))
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
                }).FirstOrDefault();
        }
        public UsersDto RegisterAuto(string email, string password, string uuid, string fromto, Guid rolesId, string nickName)
        {
            var u = new Users()
            {
                Email = email,
                Password = password,
                NickName = nickName,
                Avatar = "defaultbg.jpg",
                Image = "default.jpg",
                RolesId = rolesId,
                uuid = uuid,
                fromto = fromto
            };
             _dal.Add(u);
            return new UsersDto()
            {
                Id = u.Id,
                Email = u.Email,
                Password = u.Password,
                NickName = u.NickName,
                RolesId = u.RolesId,
                Avatar = u.Avatar,
                Image = u.Image,
                UpdateTime = u.UpdateTime
            };
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
        public async Task<bool> IsExists(string email, Guid id)
        {
            return await _dal.IsExistsAsync(ur => ur.Email.Equals(email) && ur.Id != id);
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
                    UpdateTime = data.UpdateTime,
                    Intro=data.Intro
                };
            }

            return null;
        }
        public UsersDto GetUsers(Guid id)
        {
            var data = _dal.Query().Where(x=>x.Id==id).FirstOrDefault();
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
                    UpdateTime = data.UpdateTime,
                    Intro = data.Intro
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