using EduProfileAPI.Models.Maintenance;

namespace EduProfileAPI.Repositories.Interfaces.Maintenance
{
    public interface IMaintenanceProcedure
    {

        Task<List<MaintenanceProcedure>> GetAllProceduresAsync();
        Task<bool> SaveChangesAsync();
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<MaintenanceProcedure> GetProcedureAsync(Guid maintenanceProId);
    }
}
