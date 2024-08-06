using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        [ForeignKey(nameof(ClassId))]
        public Class Class { get; set; }

        [ForeignKey(nameof(EmployeeId))]
        public Employee Employee { get; set; }

        public ICollection<StudentSubject> StudentSubjects { get; set; }

    }
}
