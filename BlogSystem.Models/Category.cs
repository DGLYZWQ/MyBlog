using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogSystem.Models
{
    public class Category : BaseEntity
    {
        [Required(ErrorMessage = "{0}不能为空"), StringLength(50), Column(TypeName = "varchar")]
        public string Title { get; set; }
        [Required(ErrorMessage = "{0}不能为空"), StringLength(1000), Column(TypeName = "varchar")]
        public string Description { get; set; }
    }
}