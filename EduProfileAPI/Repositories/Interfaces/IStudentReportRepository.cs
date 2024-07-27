using EduProfileAPI.ViewModels;
namespace EduProfileAPI.Repositories.Interfaces
{
    public interface IStudentReportRepository
    {
        Task<StudentProgressReportViewModel> GetStudentProgressReportAsync(Guid studentId);
    }
}
