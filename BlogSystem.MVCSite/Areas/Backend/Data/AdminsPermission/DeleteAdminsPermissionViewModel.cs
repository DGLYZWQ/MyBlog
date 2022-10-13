using System;
using System.ComponentModel.DataAnnotations;

namespace BlogSystem.MVCSite.Areas.Backend.Data.AdminsPermission
{
    public class DeleteAdminsPermissionViewModel
    {
        [Required]
        public Guid RolesId { get; set; }
        [Required]
        public Guid[] SystemMenuId { get; set; }
    }
}