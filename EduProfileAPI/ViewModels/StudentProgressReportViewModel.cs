using EduProfileAPI.Models;
namespace EduProfileAPI.ViewModels
{
    public class StudentProgressReportViewModel
    {
        public string StudentName { get; set; }
        //public List<AssessmentReport> Assessments { get; set; }
        public List<Merit> Merits { get; set; }
        public List<StudentIncident> Incidents { get; set; }

        //public class AssessmentReport
        //{
        //    public Assesment Assesment { get; set; }
        //    public int MarkAchieved { get; set; }
        //}
        public string ParentEmail { get; set; }
    }
}
