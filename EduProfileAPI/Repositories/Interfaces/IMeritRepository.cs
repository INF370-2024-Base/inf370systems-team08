using EduProfileAPI.Models;

namespace EduProfileAPI.Repositories.Interfaces
{
    public interface IMeritRepository
    {
        Task<Merit[]> GetAllMeritsAsync();
        Task<bool> SaveChangesAsync();
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;

        // Course
        Task<Merit> GetMeritAsync(Guid meritId);

    }
}
