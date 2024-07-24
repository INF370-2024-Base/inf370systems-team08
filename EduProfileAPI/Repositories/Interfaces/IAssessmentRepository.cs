using EduProfileAPI.ViewModels;

namespace EduProfileAPI.Repositories.Interfaces
{
    public interface IAssessmentRepository
    {
        Task<StudentAssessmentViewModel> GetStudentAssessments(Guid studentId);
    }
}
