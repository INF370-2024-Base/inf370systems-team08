using System.ComponentModel.DataAnnotations;

namespace EduProfileAPI.Models
{
    public class Class
    {
        [Key]
        public Guid ClassId { get; set; }
        public Guid GradeId { get; set; }
        public Guid EmployeeId { get; set; }


        public string ClassName { get; set; }
        public string ClassDescription { get; set; }
    }
}
