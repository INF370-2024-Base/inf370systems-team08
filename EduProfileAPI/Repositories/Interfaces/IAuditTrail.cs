using EduProfileAPI.Models;

namespace EduProfileAPI.Repositories.Interfaces
{
    public interface IAuditTrail
    {
        Task<List<AuditTrail>> GetAllAuditTrailAsync();

        Task AddAuditTrailAsync(AuditTrail auditTrail);
        Task<bool> SaveChangesAsync();
    }
}
