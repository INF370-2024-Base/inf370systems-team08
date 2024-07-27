using MimeKit.Cryptography;
using System.ComponentModel.DataAnnotations;

namespace EduProfileAPI.Models
{
    public class AssessmentDates
    {
        [Key]
        public Guid AssessmentId { get; set; }
        public Guid EmployeeId { get; set; }
        public Guid SubjectId { get; set; }
        public string AssementName { get; set; }
        public DateTime AssessmentDate { get; set; }
        public string AssessmentType { get; set; }
        public int AssessmentGrade { get; set; }
        public int AchievableMark { get; set; }
        public int AssessmentWeighting { get; set; }


    }
}
