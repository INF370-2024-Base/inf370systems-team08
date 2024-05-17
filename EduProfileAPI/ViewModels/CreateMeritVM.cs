namespace EduProfileAPI.ViewModels
{
    public class CreateMeritVM
    {
        public Guid MeritId { get; set; }
        public Guid MeritTypeId { get; set; }
        public Guid EmployeeId { get; set; }
        public string MeritName { get; set; }
        public string MeritDescription { get; set; }

    }
}
