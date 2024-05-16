using System.ComponentModel.DataAnnotations;

namespace EduProfileAPI.Models
{
    public class Grade
    {
        [Key]
        public Guid GradeId { get; set; }
        public Guid StudentEducationPhaseId { get; set; }
        //[Required]
        public string GradeLevel { get; set; }

       
        
    }
}
