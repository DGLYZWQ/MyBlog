using System;

namespace BlogSystem.Dtos
{
    public class CommentsDto
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public string UserName { get; set; }
        public Guid UserId { get; set; }
        public Guid BlogId { get; set; }
        public string BlogName { get; set; }
        public bool IsChecked { get; set; }
        public bool IsRemoved { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public string Pid { get; set; }
    }
}