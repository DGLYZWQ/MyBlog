using System;
using System.ComponentModel.DataAnnotations;

namespace BlogSystem.Models
{
    public class BaseEntity
    {
        /// <summary>
        /// 编号（主键）
        /// </summary>
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 是否删除（伪删除）
        /// </summary>
        public bool IsRemoved { get; set; }
    }
}