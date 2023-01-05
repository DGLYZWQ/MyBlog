using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.Models
{
    public class ThumbCollect : BaseEntity
    {
        public Guid UsersId { get; set; }
        public Guid BlogId { get; set; }
        /// <summary>
        /// 1点赞 2收藏
        /// </summary>
        public int OptType { get; set; }
      

    }
}
