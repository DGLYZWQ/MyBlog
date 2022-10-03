using System;

namespace BlogSystem.Dtos
{
    public class MessagesDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Tel { get; set; }
        public string Content { get; set; }
        public string IsRead { get; set; }
        public string IsRemoved { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}