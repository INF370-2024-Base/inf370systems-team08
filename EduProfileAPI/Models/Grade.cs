using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduProfileAPI.Models
{
    public class Grade
    {
        [Key]
        public Guid GradeId { get; set; }
        public Guid StudentEducationPhaseId { get; set; }
        //[Required]
        public string GradeLevel { get; set; }

        [ForeignKey(nameof(StudentEducationPhaseId))]
        public StudentEducationPhase StudentEducationPhase { get; set; }
       
        
    }
}
