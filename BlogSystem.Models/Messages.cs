using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.Models
{
    public class Messages : BaseEntity
    {
        [Required(ErrorMessage = "{0}不能为空"), StringLength(50), Column(TypeName = "varchar")]
        public string Name { get; set; }
        [Required(ErrorMessage = "{0}不能为空"), StringLength(100), Column(TypeName = "varchar")]
        public string Email { get; set; }
        [Required(ErrorMessage = "{0}不能为空"), StringLength(50), Column(TypeName = "varchar")]
        public string Tel { get; set; }
        [Required(ErrorMessage = "{0}不能为空"), StringLength(1000), Column(TypeName = "varchar")]
        public string Content { get; set; }
        public bool IsRead { get; set; }
    }
}
