namespace EduProfileAPI.ViewModels
{
    public class AssessmentMarkViewModel
    {
        public Guid AssessmentId { get; set; }
        public string AssessmentName { get; set; }
        public DateTime AssessmentDate { get; set; }
        public string AssessmentType { get; set; }
        public int? MarkAchieved { get; set; }
        public int AchievableMark { get; set; }
    }
}
