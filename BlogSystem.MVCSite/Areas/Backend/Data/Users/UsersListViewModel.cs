using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogSystem.MVCSite.Areas.Backend.Data.Users
{
    public class UsersListViewModel
    {
        public Guid Id { get; set; }
        [Required, StringLength(50), Column(TypeName = "varchar")]
        [Display(Name = "昵称")]
        public string NickName { get; set; }
        [Required, StringLength(100), Column(TypeName = "varchar")]
        [Display(Name = "电子邮件")]
        public string Email { get; set; }
        [Required, StringLength(30), Column(TypeName = "varchar")]
        [Display(Name = "密码")]
        public string Password { get; set; }
        [Required, StringLength(255), Column(TypeName = "varchar")]
         [Display(Name = "缩略图")]
        public string Avatar { get; set; } //缩略图
        [Required, StringLength(255), Column(TypeName = "varchar")]
        [Display(Name = "头像")]
        public string Image { get; set; }  //正常头像

        [Required, StringLength(255), Column(TypeName = "varchar")]
        [Display(Name="权限名称")]
        public string RolesTitle { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}