using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace EduProfileAPI.Models
{
    public class Class
    {
        [Key]
        public Guid ClassId { get; set; }
        public Guid GradeId { get; set; }
        [AllowNull]
        public Guid? EmployeeId { get; set; }


        public string ClassName { get; set; }
        public string ClassDescription { get; set; }
    }
}
