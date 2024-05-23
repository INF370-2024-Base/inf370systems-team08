using Microsoft.AspNetCore.Mvc;

namespace EduProfileAPI.ViewModels
{
    public class UpdateSubjectViewModel
    {
        public Guid SubjectId { get; set; }
        public Guid ClassId { get; set; }
        public Guid EmployeeId { get; set; }
        public string SubjectName { get; set; }
        public string SubjectDescription { get; set; }
        public string SubjectYear { get; set; }
    }
}
