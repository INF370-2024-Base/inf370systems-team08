namespace EduProfileAPI.ViewModels
{
    public class AssessmentAverageReportViewModel
    {
        public Guid AssesmentId { get; set; }
        public string AssesmentName { get; set; }
        public DateTime AssesmentDate { get; set; }
        public double AverageMark { get; set; }
    }
}
