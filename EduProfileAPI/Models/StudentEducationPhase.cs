using System.ComponentModel.DataAnnotations;

namespace EduProfileAPI.Models
{
    public class StudentEducationPhase
    {
        [Key]
        public Guid StudentEducationPhaseId{ get; set; }
        // [Required]
        public string EducationPhaseName { get; set; }
       // [Required]
        public string GradeRange { get; set; }
    }
}
