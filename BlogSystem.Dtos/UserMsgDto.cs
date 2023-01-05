using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.Dtos
{
    public class UserMsgDto
    {
        public string Contents { get; set; }
        public string Notice { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
