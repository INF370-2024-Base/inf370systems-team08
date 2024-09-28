using EduProfileAPI.Models.Maintenance;

namespace EduProfileAPI.Repositories.Interfaces.Maintenance
{
    public interface IMaintenanceProcedure
    {

        Task<List<MaintenanceProcedure>> GetAllProceduresAsync();
        Task<bool> SaveChangesAsync();
        Task AddProcedureAsync(MaintenanceProcedure procedure, Guid userId);
        Task UpdateProcedureAsync(MaintenanceProcedure updatedProcedure, MaintenanceProcedure oldProcedure, Guid userId);

        Task DeleteProcedureAsync(MaintenanceProcedure procedure, Guid userId);
        Task<MaintenanceProcedure> GetProcedureAsync(Guid maintenanceProId);
    }
}
