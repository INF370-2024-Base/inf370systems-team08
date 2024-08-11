using EduProfileAPI.Models;

namespace EduProfileAPI.ViewModels
{
    public class AssessmentAverageReportViewModel
    {
        public Guid AssesmentId { get; set; }
        public string AssesmentName { get; set; }
        public DateTime AssesmentDate { get; set; }
        public List<AssesmentMark> Marks { get; set; } // Make sure this property exists

        public double AverageMark { get; set; }
    }
}
