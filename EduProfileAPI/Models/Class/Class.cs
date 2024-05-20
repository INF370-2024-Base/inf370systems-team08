using System.ComponentModel.DataAnnotations;

namespace EduProfileAPI.Models.Class
{
    public class Class
    {
        [Key]
        public Guid ClassId { get; set; }
<<<<<<< Updated upstream
        [Required]
        public string ClassName { get; set; }
        [Required]
        public string ClassDescription { get; set; }

        public Guid GradeId { get; set; }
        public Guid EmployeeId{ get; set; }
=======
        public Guid GradeId { get; set; }
        public Guid EmployeeId { get; set; }
        public string ClassName { get; set; }
        public string ClassDescription { get; set; }
>>>>>>> Stashed changes
    }
}
