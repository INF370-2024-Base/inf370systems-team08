using System.ComponentModel.DataAnnotations;

namespace EduProfileAPI.Models
{
    public class AttendanceStatus
    {
        [Key]
        public Guid AttendanceStatusId { get; set; }
        public string StatusDescription { get; set; }
    }
}
