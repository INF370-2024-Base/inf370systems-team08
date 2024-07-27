using System.ComponentModel.DataAnnotations;

namespace EduProfileAPI.Models
{
    public class RemedialFile
    {
        [Key]
        public Guid RemFileId { get; set; }
        public Guid EmployeeId { get; set; }
        public Guid SubjectId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime Date { get; set; }

    }
}
