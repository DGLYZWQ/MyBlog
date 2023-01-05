using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.Dtos
{
    public class ThumbCollectDto
    {
        public Guid UsersId { get; set; }
        public Guid BlogId { get; set; }
        /// <summary>
        /// 1点赞 2收藏
        /// </summary>
        public int OptType { get; set; }
        public Guid Id { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateTime { get; set; } = DateTime.Now;
    }
    public class ThumbCollectBlogDto:BlogDto
    {
       
        public Guid BlogId { get; set; }
        /// <summary>
        /// 1点赞 2收藏
        /// </summary>
        public int OptType { get; set; }
        public Guid ThumbCollectId { get; set; }
        public DateTime ThumbCollectTime { get; set; }
    }
}
