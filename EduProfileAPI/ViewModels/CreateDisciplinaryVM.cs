namespace EduProfileAPI.ViewModels
{
    public class CreateDisciplinaryVM
    {
        public Guid DisciplinaryTypeId { get; set; }
        public Guid EmployeeId { get; set; }
        public Guid StudentId { get; set; }
        public string DisciplinaryName { get; set; }
        public string DisciplinaryDescription { get; set; }
    }
}
