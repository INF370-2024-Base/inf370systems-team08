using EduProfileAPI.ViewModels;

namespace EduProfileAPI.Repositories.Interfaces
{
    public interface IAssessmentsReportsRepo
    {
        Task<List<AssessmentAverageReportViewModel>> GetAssessmentAverageReport();
    }
}
