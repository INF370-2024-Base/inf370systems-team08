using EduProfileAPI.DataAccessLayer;
using EduProfileAPI.Models;
using EduProfileAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EduProfileAPI.Repositories.Implementation
{
    public class AuditTrailRepo : IAuditTrail
    {
        private readonly EduProfileDbContext _context;
        public AuditTrailRepo(EduProfileDbContext context)
        {
            _context = context;
        }

        public async Task<List<AuditTrail>> GetAllAuditTrailAsync()
        {
            return await _context.AuditTrail
                .OrderByDescending(a => a.TimeStamp)  // Order by the most recent actions first
                .ToListAsync();  // Return all audit trail entries
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
