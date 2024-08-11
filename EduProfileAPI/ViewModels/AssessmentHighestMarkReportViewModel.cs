using EduProfileAPI.Models;

namespace EduProfileAPI.ViewModels
{
    public class AssessmentHighestMarkReportViewModel
    {
        public Guid SubjectId { get; set; }
        public Guid AssesmentId { get; set; }
        public string AssesmentName { get; set; }
        public int HighestMark { get; set; }
        public int Total { get; set; }
        public List<AssesmentMark> Marks { get; set; } // Make sure this property exists

        public string StudentFirstName { get; set; }
        public string StudentLastName { get; set; }
    }
}
