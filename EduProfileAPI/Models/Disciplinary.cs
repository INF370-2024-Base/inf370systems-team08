using System.ComponentModel.DataAnnotations;

namespace EduProfileAPI.Models
{
    public class Disciplinary
    {
        [Key]
        public Guid DisciplinaryId { get; set; }
        public Guid DisciplinaryTypeId { get; set; }
        public Guid EmployeeId { get; set; }
        public Guid StudentId { get; set; }


        //[Required]
        [StringLength(50)]
        public string Reason { get; set; }

        public bool? ParentContacted { get; set; }

        [StringLength(10)]
        public string DisciplinaryDuration { get; set; }

        public DateTime? IssueDate { get; set; }
    }
}
