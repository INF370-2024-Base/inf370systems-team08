using EduProfileAPI.Models;
using EduProfileAPI.Models.Maintenance;

namespace EduProfileAPI.Repositories.Interfaces.Maintenance
{
    public interface IMaintenanceRequest
    {
        Task<List<MaintenanceRequest>> GetAllRequestsAsync();
        Task<bool> SaveChangesAsync();
        Task AddRequestAsync(MaintenanceRequest request, Guid userId);
        Task UpdateRequestAsync(MaintenanceRequest updatedRequest, MaintenanceRequest oldRequest, Guid userId);

        Task DeleteRequestAsync(MaintenanceRequest request, Guid userId);
        Task<MaintenanceRequest> GetRequestAsync(Guid maintenanceReqId);
    }
}
