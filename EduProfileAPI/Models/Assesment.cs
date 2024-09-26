using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduProfileAPI.Models
{
    public class Assesment
    {
        [Key]
        public Guid AssesmentId { get; set; }
        public Guid SubjectId { get; set; }
        public Guid EmployeeId { get; set; }
        public string AssesmentName { get; set; } 
        public DateTime AssesmentDate { get; set; }
        public string AssesmentType { get; set; }
        public int AchievableMark { get; set; }
        public int AssesmentWeighting { get; set; }

        [ForeignKey("Term")]
        public Guid TermId { get; set; }
        public AssesmentTerm Term { get; set; }
    }
}
