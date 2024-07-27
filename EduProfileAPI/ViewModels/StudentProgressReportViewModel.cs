using EduProfileAPI.Models;
namespace EduProfileAPI.ViewModels
{
    public class StudentProgressReportViewModel
    {
        public string StudentName { get; set; }
        public List<Assesment> Assessments { get; set; }
        public List<Merit> Merits { get; set; }
        public List<StudentIncident> Incidents { get; set; }
    }
}
