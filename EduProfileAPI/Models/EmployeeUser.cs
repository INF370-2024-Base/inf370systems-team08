using System.ComponentModel.DataAnnotations;

namespace EduProfileAPI.Models
{
    public class EmployeeUser
    {
        [Key]
        public Guid UserId { get; set; }
        public Guid EmployeeId { get; set; }
        public string Description { get; set; }
    }
}
