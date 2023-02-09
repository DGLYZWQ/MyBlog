using System.ComponentModel.DataAnnotations;

namespace BlogSystem.MVCSite.Areas.Backend.Views.CategoryBackend
{
    public class AddCategoryViewModel
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }

    }
}