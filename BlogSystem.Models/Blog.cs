using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.Models
{
    public class Blog : BaseEntity

    {
        [Required(ErrorMessage="{0}不能为空"), StringLength(50), Column(TypeName = "varchar")]
        public string Title { get; set; }
        [Required(ErrorMessage = "{0}不能为空"),  Column(TypeName = "text")]
        public string Content { get; set; }
        [ForeignKey(nameof(Users))]
        public Guid UsersId { get; set; }

        public Users Users { get; set; }
        [ForeignKey(nameof(Category))]
        public Guid CategoryId { get; set; }

        public Category Category { get; set; }
        public int Views { get; set; }
        public int Comments { get; set; }
        public bool IsPublic { get; set; }
    }
}
