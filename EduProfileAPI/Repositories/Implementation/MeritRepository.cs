using EduProfileAPI.DataAccessLayer;
using EduProfileAPI.Models;
using EduProfileAPI.Models.User;
using EduProfileAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json; // For JSON serialization

namespace EduProfileAPI.Repositories.Implementation
{
    public class MeritRepository : IMeritRepository
    {
        private readonly EduProfileDbContext _context;
        private readonly IAuditTrail _auditTrailRepository; // Injecting AuditTrail repo

        public MeritRepository(EduProfileDbContext context, IAuditTrail auditTrailRepository)
        {
            _context = context;
            _auditTrailRepository = auditTrailRepository; // Assign injected dependency

        }

        // Call the stored procedure to retrieve all merits
        public async Task<List<Merit>> GetAllMeritsAsync()
        {
            return await _context.Merit
                .FromSqlRaw("EXEC GetAllMerits")
                .ToListAsync();
        }

        public async Task<Merit> GetMeritAsync(Guid meritId)
        {
            IQueryable<Merit> query = _context.Merit.Where(c => c.MeritId == meritId);
            return await query.FirstOrDefaultAsync();
        }

        // MeritRepository.cs
        public async Task AddMeritAsync(Merit merit, Guid userId)
        {
            _context.Add(merit);

            // Log the audit trail
            var auditTrail = new AuditTrail
            {
                UserId = userId,
                Action = "CREATE",
                EntityName = "Merit",
                AffectedEntityID = merit.MeritId,
                NewValue = JsonConvert.SerializeObject(merit),  // Convert merit to JSON
                TimeStamp = DateTime.UtcNow
            };

            await _auditTrailRepository.AddAuditTrailAsync(auditTrail);  // Ensure this method returns Task
        }

        public async Task UpdateMeritAsync(Merit updatedMerit, Merit oldMerit, Guid userId)
        {
            // Update merit with new values
            _context.Entry(oldMerit).CurrentValues.SetValues(updatedMerit);

            // Log the audit trail for the update
            var auditTrail = new AuditTrail
            {
                UserId = userId,
                Action = "UPDATE",
                EntityName = "Merit",
                AffectedEntityID = updatedMerit.MeritId,
                OldValue = JsonConvert.SerializeObject(oldMerit),  // Serialize the old merit data to JSON
                NewValue = JsonConvert.SerializeObject(updatedMerit),  // Serialize the new merit data to JSON
                TimeStamp = DateTime.UtcNow
            };

            // Add the audit trail to the context
            await _auditTrailRepository.AddAuditTrailAsync(auditTrail);
        }


        public async Task DeleteMeritAsync(Merit merit, Guid userId)
        {
            // Remove the merit from the context
            _context.Merit.Remove(merit);

            // Log the audit trail for the deletion
            var auditTrail = new AuditTrail
            {
                UserId = userId,
                Action = "DELETE",
                EntityName = "Merit",
                AffectedEntityID = merit.MeritId,
                OldValue = JsonConvert.SerializeObject(merit),  // Serialize the old merit data to JSON
                TimeStamp = DateTime.UtcNow  // No new value since it's a deletion
            };

            // Add the audit trail to the context
            await _auditTrailRepository.AddAuditTrailAsync(auditTrail);
        }


        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
