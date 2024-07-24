using System.ComponentModel.DataAnnotations;

namespace EduProfileAPI.Models
{
    public class Assesment
    {
        [Key]
        public Guid AssesmentId { get; set; }
        public Guid SubjectId { get; set; }
        public Guid EmployeeId { get; set; }
        public string AssessmentName { get; set; } 
        public string AssesmentDate { get; set; }
        public string AssesmentType { get; set; }
        public string AchievableMark { get; set; }
        public string AssesmentWeighting { get; set; }






    }
}
