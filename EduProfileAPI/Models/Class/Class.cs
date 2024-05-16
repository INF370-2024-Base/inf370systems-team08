using System.ComponentModel.DataAnnotations;

namespace EduProfileAPI.Models.Class
{
    public class Class
    {
        [Key]
        public Guid ClassId { get; set; }
        [Required]
        public string ClassName { get; set; }
        [Required]
        public string ClassDescription { get; set; }

        public Guid GradeId { get; set; }
        public Guid EmployeeId{ get; set; }
    }
}
