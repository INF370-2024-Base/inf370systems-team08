using System.Diagnostics.CodeAnalysis;

namespace EduProfileAPI.ViewModels
{
    public class ClassVM
    {
        public Guid GradeId { get; set; }
        [AllowNull]
        public Guid? EmployeeId { get; set; }
        public string ClassName { get; set; }
        public string ClassDescription { get; set; }
       
    }
}
