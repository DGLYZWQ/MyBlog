using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogSystem.MVCSite.Areas.Backend.Data.Blogs
{
    public class BlogsListViewModel
    {
        public Guid Id { get; set; }
        [Display(Name = "博客名称")]
        public string Title { get; set; }
        [Display(Name = "博客类别")]
        public string CategoryName { get; set; }
        public Guid CategoryId { get; set; }
        public string UserName { get; set; }
        public string Content { get; set; }
        public Guid UsersId { get; set; }

        public int Views { get; set; }
        public int Comments { get; set; }

        public int Thumps { get; set; }
        [Display(Name = "是否公开")]
        public bool IsPublic { get; set; }
        [Display(Name = "修改时间")]
        public DateTime UpdateTime { get; set; }
    }
}