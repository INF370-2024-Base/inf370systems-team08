using System.ComponentModel.DataAnnotations;

namespace EduProfileAPI.ViewModels
{
    public class AssesmentMarkVM
    {
        [Key]
        public Guid StudentId { get; set; }
        [Key]
        public Guid AssesmentId { get; set; }

        public int MarkAchieved { get; set; }
    }
}
