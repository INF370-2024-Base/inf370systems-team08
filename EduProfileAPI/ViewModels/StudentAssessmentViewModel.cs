namespace EduProfileAPI.ViewModels
{
    public class StudentAssessmentViewModel
    {
        public Guid StudentId { get; set; }
        public string StudentName { get; set; }
        public List<AssessmentMarkViewModel> AssessmentMarks { get; set; }
    }
}
