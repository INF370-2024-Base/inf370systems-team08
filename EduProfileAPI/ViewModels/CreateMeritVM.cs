namespace EduProfileAPI.ViewModels
{
    public class CreateMeritVM
    {
        public Guid MeritTypeId { get; set; }
        public Guid EmployeeId { get; set; }
        public Guid StudentId { get; set; }
        public string MeritName { get; set; }
        public string MeritDescription { get; set; }


    }
}
