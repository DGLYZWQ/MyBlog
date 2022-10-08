using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace BlogSystem.MVCSite.Areas.Backend.Data.Roles
{
    public class AddRolesViewModel
    {
        [Required(ErrorMessage="{0}不能为空"),StringLength(255),Column(TypeName="varchar")]
        [Display(Name="权限名称")]
        [Remote("CheckRolesTitle","RolesBackend",ErrorMessage = "该权限名称已存在")]
        public string Title { get; set; }
    }
}