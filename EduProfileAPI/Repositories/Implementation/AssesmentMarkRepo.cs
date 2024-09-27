using EduProfileAPI.DataAccessLayer;
using EduProfileAPI.Models;
using EduProfileAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace EduProfileAPI.Repositories.Implementation
{
    public class AssesmentMarkRepo: IAssesmentMark
    {
        private readonly EduProfileDbContext _context;
        private readonly IAuditTrail _auditTrailRepository;

        public AssesmentMarkRepo(EduProfileDbContext context, IAuditTrail auditTrailRepository)
        {
            _context = context;
            _auditTrailRepository = auditTrailRepository;
        }

        public async Task<AssesmentMark[]> GetAllAssesmentMarksAsync()
        {
            IQueryable<AssesmentMark> query = _context.AssesmentMark;
            return await query.ToArrayAsync();
        }

        public async Task<AssesmentMark> GetAssesmentMarkAsync(Guid studentId, Guid assesmentId)
        {
            IQueryable<AssesmentMark> query = _context.AssesmentMark.Where(c => c.StudentId  == studentId && c.AssesmentId == assesmentId);
            return await query.FirstOrDefaultAsync();
        }

        // Add a new assessment mark and log the action
        public async Task AddAssesmentMarkAsync(AssesmentMark assesmentMark, Guid userId)
        {
            _context.AssesmentMark.Add(assesmentMark);

            // Log the "CREATE" action in the audit trail
            var auditTrail = new AuditTrail
            {
                UserId = userId,
                Action = "CREATE",
                EntityName = "AssesmentMark",
                AffectedEntityID = assesmentMark.AssesmentId,
                NewValue = JsonConvert.SerializeObject(assesmentMark),
                TimeStamp = DateTime.UtcNow // You can adjust to South Africa Standard Time
            };

            _context.AuditTrail.Add(auditTrail);
        }

        // Update an existing assessment mark and log the action
        public async Task UpdateAssesmentMarkAsync(AssesmentMark updatedMark, AssesmentMark oldMark, Guid userId)
        {
            _context.Entry(oldMark).CurrentValues.SetValues(updatedMark);

            // Log the "UPDATE" action in the audit trail
            var auditTrail = new AuditTrail
            {
                UserId = userId,
                Action = "UPDATE",
                EntityName = "AssesmentMark",
                AffectedEntityID = updatedMark.AssesmentId,
                OldValue = JsonConvert.SerializeObject(oldMark),
                NewValue = JsonConvert.SerializeObject(updatedMark),
                TimeStamp = DateTime.UtcNow // Adjust to South African time if necessary
            };

            _context.AuditTrail.Add(auditTrail);
        }


        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
