using System.ComponentModel.DataAnnotations;

namespace EduProfileAPI.Models
{
    public class StudentAnnouncement
    {
        [Key]
        public Guid StudentAnnId { get; set; }
        public Guid ParentId { get; set; }
        public DateTime AnnouncementDate { get; set; }
        public string Description { get; set; }
    }
}
