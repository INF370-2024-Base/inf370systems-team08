namespace EduProfileAPI.ViewModels
{
    public class SubjectViewModel
    {
        // Change the guid of forieng keys to the the name like employee later
        public Guid SubjectId { get; set; }
        public Guid ClassId { get; set; }
        public Guid EmployeeId { get; set; }
        //public string ClassId { get; set; }
        //public string EmployeeFirstName { get; set; }
        //public string EmployeeLastName { get; set; }
        public string SubjectName { get; set; }
        public string SubjectDescription { get; set; }
        public string SubjectYear { get; set; }
    }
}
