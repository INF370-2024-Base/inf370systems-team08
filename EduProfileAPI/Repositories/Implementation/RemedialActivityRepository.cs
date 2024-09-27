using EduProfileAPI.DataAccessLayer;
using EduProfileAPI.Models;
using EduProfileAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;



namespace EduProfileAPI.Repositories.Implementation
{
    public class RemedialActivityRepository : IRemedialActivityRepository
    {
        private readonly EduProfileDbContext _context;
        private readonly IAuditTrail _auditTrailRepository;


        public RemedialActivityRepository(EduProfileDbContext context, IAuditTrail auditTrailRepository)
        {
            _context = context;
            _auditTrailRepository = auditTrailRepository;
        }

        // Call the stored procedure to retrieve all remedial activities
        public async Task<List<RemedialActivity>> GetAllRemedialActivitiesAsync()
        {
            return await _context.RemedialActivity
                .FromSqlRaw("EXEC GetAllRemedialActivities")
                .ToListAsync();
        }

        public async Task<RemedialActivity> GetRemedialActivityAsync(Guid remedialActivityId)
        {
            IQueryable<RemedialActivity> query = _context.RemedialActivity.Where(c => c.RemActId == remedialActivityId);
            return await query.FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(RemedialActivity activity)
        {
            _context.RemedialActivity.Update(activity);
            await _context.SaveChangesAsync();

        }

        public async Task AddRemedialActivityAsync(RemedialActivity activity, Guid userId)
        {
            await _context.RemedialActivity.AddAsync(activity);
            await _context.SaveChangesAsync();

            // Log the creation in the audit trail
            var auditTrail = new AuditTrail
            {
                UserId = userId,
                Action = "CREATE",
                EntityName = "RemedialActivity",
                AffectedEntityID = activity.RemActId,
                NewValue = JsonConvert.SerializeObject(activity),
                TimeStamp = DateTime.UtcNow
            };
            _context.AuditTrail.Add(auditTrail);
            await _context.SaveChangesAsync();
        }

        // Update Remedial Activity with Audit Trail
        public async Task UpdateRemedialActivityAsync(RemedialActivity activity, Guid userId)
        {
            var oldActivity = await _context.RemedialActivity.AsNoTracking().FirstOrDefaultAsync(a => a.RemActId == activity.RemActId);

            _context.RemedialActivity.Update(activity);
            await _context.SaveChangesAsync();

            // Log the update in the audit trail
            var auditTrail = new AuditTrail
            {
                UserId = userId,
                Action = "UPDATE",
                EntityName = "RemedialActivity",
                AffectedEntityID = activity.RemActId,
                OldValue = JsonConvert.SerializeObject(oldActivity),
                NewValue = JsonConvert.SerializeObject(activity),
                TimeStamp = DateTime.UtcNow
            };
            _context.AuditTrail.Add(auditTrail);
            await _context.SaveChangesAsync();
        }

        // Delete Remedial Activity with Audit Trail
        public async Task DeleteRemedialActivityAsync(RemedialActivity activity, Guid userId)
        {
            _context.RemedialActivity.Remove(activity);
            await _context.SaveChangesAsync();

            // Log the delete in the audit trail
            var auditTrail = new AuditTrail
            {
                UserId = userId,
                Action = "DELETE",
                EntityName = "RemedialActivity",
                AffectedEntityID = activity.RemActId,
                OldValue = JsonConvert.SerializeObject(activity),
                TimeStamp = DateTime.UtcNow
            };
            _context.AuditTrail.Add(auditTrail);
            await _context.SaveChangesAsync();
        }


        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<RemedialActivity>> GetActivitiesByFileId(Guid remFileId)
        {
            return await _context.RemedialActivity
                                 .Where(a => a.RemFileId == remFileId)
                                 .ToListAsync();
        }

    }

}
