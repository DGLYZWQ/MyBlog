using System;
using System.ComponentModel.DataAnnotations;

namespace BlogSystem.MVCSite.Models.dtos
{
    public class UpdateUsersDto
    {
        public Guid Id { get; set; }
        public string NickName { get; set; }
        [Required(ErrorMessage = "{0}不能为空")]
        [Display(Name = "电子邮箱")]
        [EmailAddress(ErrorMessage = "邮箱地址格式不正确")]
        [System.Web.Mvc.Remote("CheckEmailUpdateAsync", "FrontHome", ErrorMessage = "该电子邮箱已被使用")]
        public string Email { get; set; }
        [Required(ErrorMessage = "{0}不能为空")]
        [Display(Name = "密码")]
        [StringLength(16, MinimumLength = 6, ErrorMessage = "密码长度不正确")]
        public string Password { get; set; }
       
        public string Avatar { get; set; }
        public string Image { get; set; }
        public string Intro { get; set; }


    }
}