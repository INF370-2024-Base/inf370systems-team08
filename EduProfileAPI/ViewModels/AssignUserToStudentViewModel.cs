namespace EduProfileAPI.ViewModels
{
    public class AssignUserToStudentViewModel
    {
        public Guid UserId { get; set; }
        public Guid StudentId { get; set; }
        public string Description { get; set; }
    }
}
