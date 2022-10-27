using System;
using System.ComponentModel.DataAnnotations;

namespace BlogSystem.MVCSite.Areas.Backend.Data.Blogs
{
    public class AddNoticeViewModel
    {
        [Required]
        public string Title { get; set; }
        public string Category { get; set; }
        public Guid CategoryId { get; set; }
        [Required] 
        public string Content { get; set; }
    }
}