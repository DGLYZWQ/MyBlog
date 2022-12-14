using System;
using System.ComponentModel.DataAnnotations;

namespace BlogSystem.MVCSite.Areas.Backend.Data.Login
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "{0}不能为空")]
        [Display(Name = "电子邮箱")]
        [EmailAddress(ErrorMessage = "邮箱地址格式不正确")]
        public string Email { get; set; }
        [Required(ErrorMessage = "{0}不能为空")]
        [Display(Name = "密码")]
        [StringLength(16, MinimumLength = 6, ErrorMessage = "密码长度不正确")]
        public string Password { get; set; }
        [Required]
        [Compare(nameof(Password),ErrorMessage ="两次密码输入不一致")]
        public string ConfirmPassword { get; set; }
        public Guid RolesId { get; set; }
    }
}