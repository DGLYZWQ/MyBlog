using System;
using System.ComponentModel.DataAnnotations;

namespace BlogSystem.MVCSite.Areas.Backend.Data.Category
{
    public class CategoryListViewModel
    {
        public Guid Id { get; set; }
        [Display(Name = "博客名称")]
        public string Title { get; set; }
        public string Description { get; set; }

        public DateTime UpdateTime { get; set; }
    }
}