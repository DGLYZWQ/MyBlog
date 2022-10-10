using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace BlogSystem.MVCSite.Areas.Backend.Data.Users
{
    public class AddUsersViewModel
    {
        [Required, StringLength(50), Column(TypeName = "varchar")]
        public string NickName { get; set; }
        [Required, StringLength(100), Column(TypeName = "varchar")]
        [Remote("CheckEmailAsync","UsersBackend",ErrorMessage = "该电子邮箱已被使用")]
        public string Email { get; set; }
        [Required, StringLength(30), Column(TypeName = "varchar")]
        public string Password { get; set; }
        [StringLength(255), Column(TypeName = "varchar")]
        public string Avatar { get; set; } //缩略图
        [StringLength(255), Column(TypeName = "varchar")]
        public string Image { get; set; }  //正常头像
        public int BlogCount { get; set; }
        public int CategoryCount { get; set; }
        public int FollowsCount { get; set; }
        public int FansCount { get; set; }

        [Display(Name = "权限编号")]
        [ForeignKey(nameof(Roles))]
        public Guid RolesId { get; set; }
    }
}