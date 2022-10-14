using System;

namespace BlogSystem.MVCSite.Areas.Backend.Data.Comments
{
    public class CommentsListViewModel
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public string BlogName { get; set; }
        public Guid BlogId { get; set; }
        public bool IsChecked { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}