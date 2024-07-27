using EduProfileAPI.Models;

namespace EduProfileAPI.Repositories.Interfaces
{
    public interface IReportType
    {
        Task<ReportType[]> GetAllReportTypesAsync();
        Task<bool> SaveChangesAsync();
        void Add<T>(T entity) where T : class;
        Task<ReportType> GetReportTypeAsync(Guid reportTypeId);
    }
}
