namespace EduProfileAPI.ViewModels
{
    public class CreateSubjectViewModel
    {
        public Guid EmployeeId { get; set; }
        public Guid ClassId { get; set; }
        public string SubjectName { get; set; }
        public string SubjectDescription { get; set; }  
        public string SubjectYear { get; set; }
    }
}
