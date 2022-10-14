using System;

namespace BlogSystem.MVCSite.Areas.Backend.Data.Messages
{
    public class MessagesListViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Content { get; set; }
        public bool IsRead { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}