using EduProfileAPI.Models.Maintenance;

namespace EduProfileAPI.Repositories.Interfaces.Maintenance
{
    public interface IMaintenanceStatus
    {
        Task<MaintenanceStatus[]> GetAllStatusAsync();
        Task<bool> SaveChangesAsync();
        void Add<T>(T entity) where T : class;
    }
}
