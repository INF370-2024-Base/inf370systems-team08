using EduProfileAPI.Models.Maintenance;

namespace EduProfileAPI.Repositories.Interfaces.Maintenance
{
    public interface IMaintenancePriority
    {
        Task<MaintenancePriority[]> GetAllPriorityAsync();
        Task<bool> SaveChangesAsync();
        void Add<T>(T entity) where T : class;
    }
}
