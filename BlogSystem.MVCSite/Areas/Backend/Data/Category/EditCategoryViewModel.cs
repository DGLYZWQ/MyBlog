using System;

namespace BlogSystem.MVCSite.Areas.Backend.Data.Category
{
    public class EditCategoryViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}