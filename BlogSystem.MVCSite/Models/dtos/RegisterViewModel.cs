using System;
using System.ComponentModel.DataAnnotations;

namespace BlogSystem.MVCSite.Models.dtos
{
    public class ForgetPasswordViewModel
    {
        [Required(ErrorMessage = "{0}不能为空")]
        [Display(Name = "电子邮箱")]
        [EmailAddress(ErrorMessage = "邮箱地址格式不正确")]
        [System.Web.Mvc.Remote("CheckEmailIsExit", "FrontHome", ErrorMessage = "该电子邮箱不存在")]
        public string Email { get; set; }
    }
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "{0}不能为空")]
        [Display(Name = "电子邮箱")]
        [EmailAddress(ErrorMessage = "邮箱地址格式不正确")]
        [System.Web.Mvc.Remote("CheckEmailAsync", "FrontHome", ErrorMessage = "该电子邮箱已被使用")]
        public string Email { get; set; }
        [Required(ErrorMessage = "{0}不能为空")]
        [Display(Name = "密码")]
        [StringLength(16, MinimumLength = 6, ErrorMessage = "密码长度不正确")]
        public string Password { get; set; }
        [Required(ErrorMessage = "确认密码不能为空")]
        [Compare(nameof(Password), ErrorMessage = "两次密码输入不一致")]
        public string ConfirmPassword { get; set; }
        
    }
}