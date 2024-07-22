using EduProfileAPI.Models;

namespace EduProfileAPI.Repositories.Interfaces
{
    public interface IRemedialActivityRepository
    {
        Task<RemedialActivity[]> GetAllRemedialActivitiesAsync();
        Task<bool> SaveChangesAsync();
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<RemedialActivity> GetRemedialActivityByIdAsync(Guid id);
    }
}
