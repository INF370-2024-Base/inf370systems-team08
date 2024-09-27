using EduProfileAPI.Models;

namespace EduProfileAPI.Repositories.Interfaces
{
    public interface IAuditTrail
    {
        Task AddAuditTrailAsync(AuditTrail auditTrail);
        Task<bool> SaveChangesAsync();
    }
}
