using System.ComponentModel.DataAnnotations;

namespace EduProfileAPI.Models
{
    public class AssesmentMark
    {
        
        public Guid StudentId { get; set; }
        
        public Guid AssesmentId { get; set; }
        public int MarkAchieved { get; set; }

    }
}
