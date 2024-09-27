namespace EduProfileAPI.ViewModels
{
    public class AssesmentVM
    {
        public Guid SubjectId { get; set; }
        public Guid EmployeeId { get; set; }
        public string AssesmentName { get; set; }
        public DateTime AssesmentDate { get; set; }
        public string AssesmentType { get; set; }
        public int AchievableMark { get; set; }
        public int AssesmentWeighting { get; set; }
        //public int Term { get; set; }
        public Guid TermId { get; set; }
    }
}
