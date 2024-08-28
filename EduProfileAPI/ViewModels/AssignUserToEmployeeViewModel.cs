namespace EduProfileAPI.ViewModels
{
    public class AssignUserToEmployeeViewModel
    {
        public Guid UserId { get; set; }
        public Guid EmployeeId { get; set; }
        public string Description { get; set; }
    }
}
