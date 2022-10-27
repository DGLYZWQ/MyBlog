using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.Models
{
    public class Views : BaseEntity
    {
        
      
        [ForeignKey(nameof(Blog))]
        public Guid BlogId { get; set; }
        public Guid? UserId { get; set; }
        public string IP { get; set; }
        public Blog Blog { get; set; }
    }
}
