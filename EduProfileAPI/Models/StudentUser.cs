using System.ComponentModel.DataAnnotations;

namespace EduProfileAPI.Models
{
    public class StudentUser
    {
        [Key]
        public Guid UserId { get; set; }
        public Guid StudentId { get; set; }
        public string Description { get; set; }
    }
}
