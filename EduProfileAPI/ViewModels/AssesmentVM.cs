namespace EduProfileAPI.ViewModels
{
    public class AssesmentVM
    {
        public Guid SubjectId { get; set; }
        public Guid EmployeeId { get; set; }
        public string AssessmentName { get; set; }
        public string AssesmentDate { get; set; }
        public string AssesmentType { get; set; }
        public string AchievableMark { get; set; }
        public string AssesmentWeighting { get; set; }
    }
}
