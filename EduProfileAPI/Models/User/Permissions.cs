using System.ComponentModel.DataAnnotations;

namespace EduProfileAPI.Models.User
{
    public class Permissions
    {
        [Key]
        public int PermissionId { get; set; }
        public string Name { get; set; }
    }
}
