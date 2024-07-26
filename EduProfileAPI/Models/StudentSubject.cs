using System.ComponentModel.DataAnnotations;

namespace EduProfileAPI.Models
{
    public class StudentSubject
    {
        [Key]
        public Guid StudentId { get; set; }
        public Student Student { get; set; }

        public Guid SubjectId { get; set; }
        public Subject Subject { get; set; }
    }
}
