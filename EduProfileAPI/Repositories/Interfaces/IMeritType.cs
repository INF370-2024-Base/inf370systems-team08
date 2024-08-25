using EduProfileAPI.Models;

namespace EduProfileAPI.Repositories.Interfaces
{
    public interface IMeritType
    {
        Task<MeritType[]> GetAllMeritTypesAsync();
        Task<bool> SaveChangesAsync();
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<MeritType> GetMeritTypeAsync(Guid meritTypeId);
    }
}
