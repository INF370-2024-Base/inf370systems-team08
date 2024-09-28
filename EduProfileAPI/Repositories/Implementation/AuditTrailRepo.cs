using EduProfileAPI.DataAccessLayer;
using EduProfileAPI.Models;
using EduProfileAPI.Repositories.Interfaces;

namespace EduProfileAPI.Repositories.Implementation
{
    public class AuditTrailRepo : IAuditTrail
    {
        private readonly EduProfileDbContext _context;
        public AuditTrailRepo(EduProfileDbContext context)
        {
            _context = context;
        }

        // SaveChangesAsync should return Task<bool> or Task<int>
        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        // AddAuditTrailAsync should return Task
        public async Task AddAuditTrailAsync(AuditTrail auditTrail)
        {
            _context.AuditTrail.Add(auditTrail);
            await _context.SaveChangesAsync();
        }


    }
}
