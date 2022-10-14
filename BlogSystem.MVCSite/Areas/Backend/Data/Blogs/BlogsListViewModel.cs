﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogSystem.MVCSite.Areas.Backend.Data.Blogs
{
    public class BlogsListViewModel
    {
        public Guid Id { get; set; }
        [Display(Name = "博客名称")]
        public string Title { get; set; }
        public Guid CategoryId { get; set; }
        [Display(Name = "博客类别")]
        public string CategoryName { get; set; }
        [Display(Name = "是否公开")]
        public bool IsPublic { get; set; }
        [Display(Name = "修改时间")]
        public DateTime UpdateTime { get; set; }
    }
}