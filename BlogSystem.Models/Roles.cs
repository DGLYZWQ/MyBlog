using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.Models
{
    public class Roles : BaseEntity
    {
        [Required,StringLength(100),Column(TypeName="varchar")]
        [Display(Name="权限名称")]
        public string Title { get; set; }
    }
}
