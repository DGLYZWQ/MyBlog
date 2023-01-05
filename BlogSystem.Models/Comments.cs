using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.Models
{
    public class Comments : BaseEntity
    {
        [Required(ErrorMessage = "{0}不能为空")]
        public string Content { get; set; }
        [ForeignKey(nameof(Blog))]
        public Guid BlogId { get; set; }
        public Guid UserId { get; set; }

        public Blog Blog { get; set; }
        public bool IsChecked { get; set; }

        public string Pid { get; set; }
    }
}
