using System.ComponentModel.DataAnnotations;

namespace EduProfileAPI.Models.User
{
    public class UserPermissions
    {
        [Key]
        public int UserPermissionId { get; set; }
        public Guid Id { get; set; }
        public int PermissionId { get; set; }

    }
}
