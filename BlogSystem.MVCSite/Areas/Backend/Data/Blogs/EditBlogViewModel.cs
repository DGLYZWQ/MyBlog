using System;

namespace BlogSystem.MVCSite.Areas.Backend.Data.Blogs
{
    public class EditBlogViewModel
    {
        public Guid BlogId { get; set; }
        public string BlogName { get; set; }
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }
        public bool IsPublic { get; set; }
    }
}