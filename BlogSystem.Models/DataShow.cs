using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogSystem.Models
{
    public class DataShow : BaseEntity
    {
        [Required(ErrorMessage = "{0}不能为空"), StringLength(50), Column(TypeName = "varchar")]
        public string Title { get; set; }
        [Required(ErrorMessage = "{0}不能为空"), StringLength(50), Column(TypeName = "varchar")]
        public string Icons { get; set; }
        public int Views { get; set; }
        public int Comments { get; set; }
        public int Messages { get; set; }
    }
}