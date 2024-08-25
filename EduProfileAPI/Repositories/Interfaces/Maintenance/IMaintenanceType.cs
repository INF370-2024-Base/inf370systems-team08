using EduProfileAPI.Models;
using EduProfileAPI.Models.Maintenance;

namespace EduProfileAPI.Repositories.Interfaces.Maintenance
{
    public interface IMaintenanceType
    {
        Task<MaintenanceType[]> GetAllTypesAsync();
        Task<bool> SaveChangesAsync();
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<MaintenanceType> GetMaintenanceType(Guid id);

    }
}
