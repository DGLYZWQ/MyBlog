using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.Models
{
    [Table("UserMsg")]
    public class UserMsg : BaseEntity
    {
        
        public Guid UserId { get; set; }
        public string Contents { get; set; }
        public bool IsRead { get; set; }
    }
}
