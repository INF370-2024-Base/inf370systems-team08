
namespace EduProfileAPI.Models.Class
{
    public class ClassVM
    {
        public Guid ClassId { get; set; }
        public string ClassName { get; set; }
        public string ClassDescription { get; set; }
        public Guid GradeId { get; set; }
        public Guid EmployeeId { get; set; }
    }
}
