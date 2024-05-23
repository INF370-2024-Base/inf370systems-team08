using System.ComponentModel.DataAnnotations;

namespace EduProfileAPI.Models
{
    public class Subject
    {
        [Key]
        public Guid SubjectId { get; set; }
        public Guid ClassId { get; set; }
        public Guid EmployeeId { get; set; }
        public string SubjectName { get; set; }
        public string SubjectDescription { get; set; }
        public string SubjectYear { get; set; }
        
    }
}
