using System.ComponentModel.DataAnnotations;

namespace BlogSystem.MVCSite.Areas.Backend.Data.Login
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "{0}不能为空")]
        [Display(Name="电子邮箱")]
        [EmailAddress(ErrorMessage = "邮箱地址格式不正确")]
        public string Email { get; set; }
        [Required(ErrorMessage = "{0}不能为空")]
        [Display(Name = "密码")]
        [StringLength(16,MinimumLength = 6, ErrorMessage = "密码长度不正确")]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}