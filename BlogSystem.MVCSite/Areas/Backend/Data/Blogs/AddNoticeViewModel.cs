using System;

namespace BlogSystem.MVCSite.Areas.Backend.Data.Blogs
{
    public class AddNoticeViewModel
    {
        public string Title { get; set; }
        public string Category { get; set; }
        public Guid CategoryId { get; set; }
        public string Content { get; set; }
    }
}