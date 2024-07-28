namespace EduProfileAPI.ViewModels
{
    public class AssessmentHighestMarkReportViewModel
    {
        public Guid SubjectId { get; set; }
        public Guid AssesmentId { get; set; }
        public string AssesmentName { get; set; }
        public int HighestMark { get; set; }
        public string StudentFirstName { get; set; }
        public string StudentLastName { get; set; }
    }
}
