using System;

namespace BlogSystem.MVCSite.Areas.Backend.Data.SystemMenu
{
    public class SystemMenuListViewModel
    {
        public Guid Id { get; set; }
        public DateTime UpdateTime { get; set; }
        public string Title { get; set; }
        public string Link { get; set; }
        public string Icon { get; set; }
        public string ParentTitle { get; set; }
    }
}