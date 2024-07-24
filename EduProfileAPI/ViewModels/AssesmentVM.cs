namespace EduProfileAPI.ViewModels
{
    public class AssesmentVM
    {
        public Guid SubjectId { get; set; }
        public Guid EmployeeId { get; set; }
        public string AssessmentName { get; set; }
        public DateTime AssesmentDate { get; set; }
        public string AssesmentType { get; set; }
        public int AchievableMark { get; set; }
        public int AssesmentWeighting { get; set; }
    }
}
