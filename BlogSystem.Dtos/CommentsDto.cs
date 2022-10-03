using System;

namespace BlogSystem.Dtos
{
    public class CommentsDto
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public Guid BlogId { get; set; }
        public bool IsChecked { get; set; }
        public bool IsRemoved { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}