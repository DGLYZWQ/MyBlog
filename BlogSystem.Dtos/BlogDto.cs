using System;

namespace BlogSystem.Dtos
{
    public class BlogDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public Guid UsersId { get; set; }
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int Views { get; set; }
        public int Comments { get; set; }
        public bool IsPublic { get; set; }
        public bool IsRemoved { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }

    }
}