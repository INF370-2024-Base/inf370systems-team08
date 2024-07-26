using EduProfileAPI.Models;

namespace EduProfileAPI.Repositories.Interfaces
{
    public interface IReport
    {
        Task<Report[]> GetAllReportsAsync();
        Task<bool> SaveChangesAsync();
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<Report> GetReportAsync(Guid reportId);
    }
}
