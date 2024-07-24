using System.ComponentModel.DataAnnotations;

namespace EduProfileAPI.Models
{
    public class AssesmentMark
    {
        [Key]
        public Guid StudentId { get; set; }
        [Key]
        public Guid AssementId { get; set; }

        public int MarkAchieved { get; set; }

    }
}
