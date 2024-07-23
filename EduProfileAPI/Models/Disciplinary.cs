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
        public string DisciplinaryName { get; set; }
        public string DisciplinaryDescription { get; set; }
    }
}
