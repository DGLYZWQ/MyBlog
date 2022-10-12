using System;

namespace BlogSystem.Dtos
{
    public class WebMenuDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public DateTime UpdateTime { get; set; }
        public string Link { get; set; }
        public string Icon { get; set; }
        public Guid ParentId { get; set; }
        public bool IsRemoved { get; set; }
        public DateTime CreateTime { get; set; }
    }
}