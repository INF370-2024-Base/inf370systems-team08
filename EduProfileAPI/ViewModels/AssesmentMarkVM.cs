using System.ComponentModel.DataAnnotations;

namespace EduProfileAPI.ViewModels
{
    public class AssesmentMarkVM
    {
        [Key]
        public Guid StudentId { get; set; }
        [Key]
        public Guid AssementId { get; set; }

        public int MarkAchieved { get; set; }
    }
}
