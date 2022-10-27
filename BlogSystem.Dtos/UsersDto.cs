using System;

namespace BlogSystem.Dtos
{
    public class UsersDto
    {
        public Guid Id { get; set; }
        public string NickName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Intro { get; set; }
        public Guid RolesId { get; set; }
        public string Avatar { get; set; }
        public string Image { get; set; }
        public int BlogCount { get; set; }
        public int CategoryCount { get; set; }
        public int FollowsCount { get; set; }
        public int FansCount { get; set; }
        public bool IsRemoved { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }

    }
}