using EduProfileAPI.DataAccessLayer;
using EduProfileAPI.Models;
using EduProfileAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
namespace EduProfileAPI.Repositories.Implementation
{
    public class AssesmentRepo: IAssesment
    {
        private readonly EduProfileDbContext _context;
        private readonly ILogger<AssesmentRepo> _logger;
        private readonly IAuditTrail _auditTrailRepository; // Inject the audit trail repository

        public AssesmentRepo(EduProfileDbContext context, ILogger<AssesmentRepo> logger, IAuditTrail auditTrailRepository)
        {
            _context = context;
            _logger = logger;
            _auditTrailRepository = auditTrailRepository;
        }

        public async Task<Assesment[]> GetAllAssesmentsAsync()
        {
            try
            {
                // Fetch assessments, handle null checks safely
                var assessments = await _context.Assesment
                                                .ToArrayAsync();

                // Return an empty array if assessments are null
                return assessments ?? new Assesment[0];
            }
            catch (Exception ex)
            {
                // Log the error
                _logger.LogError(ex, "Error retrieving assessments.");
                throw;
            }
        }


        public async Task<Assesment> GetAssesmentAsync(Guid assesmentId)
        {
            IQueryable<Assesment> query = _context.Assesment.Where(c => c.AssesmentId == assesmentId);
            return await query.FirstOrDefaultAsync();
        }

        // Add a new assessment and log the action
        public async Task AddAssessmentAsync(Assesment assesment, Guid userId)
        {
            _context.Add(assesment);

            var auditTrail = new AuditTrail
            {
                UserId = userId,
                Action = "CREATE",
                EntityName = "Assesment",
                AffectedEntityID = assesment.AssesmentId,
                NewValue = JsonConvert.SerializeObject(assesment), // Log the new assessment data
                TimeStamp = DateTime.UtcNow
            };

            await _auditTrailRepository.AddAuditTrailAsync(auditTrail);
        }

        // Update an existing assessment and log the action
        public async Task UpdateAssessmentAsync(Assesment updatedAssesment, Assesment oldAssesment, Guid userId)
        {
            _context.Entry(oldAssesment).CurrentValues.SetValues(updatedAssesment);

            var auditTrail = new AuditTrail
            {
                UserId = userId,
                Action = "UPDATE",
                EntityName = "Assesment",
                AffectedEntityID = updatedAssesment.AssesmentId,
                OldValue = JsonConvert.SerializeObject(oldAssesment), // Log the old data
                NewValue = JsonConvert.SerializeObject(updatedAssesment), // Log the new data
                TimeStamp = DateTime.UtcNow
            };

            await _auditTrailRepository.AddAuditTrailAsync(auditTrail);
        }

        // Delete an assessment and log the action
        public async Task DeleteAssessmentAsync(Assesment assesment, Guid userId)
        {
            _context.Assesment.Remove(assesment);

            var auditTrail = new AuditTrail
            {
                UserId = userId,
                Action = "DELETE",
                EntityName = "Assesment",
                AffectedEntityID = assesment.AssesmentId,
                OldValue = JsonConvert.SerializeObject(assesment), // Log the old data
                TimeStamp = DateTime.UtcNow
            };

            await _auditTrailRepository.AddAuditTrailAsync(auditTrail);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Assesment[]> GetAssessmentsByTermAsync(int term)
        {
            return await _context.Assesment
                                 .Where(a => a.Term == term)
                                 .ToArrayAsync();
        }

    }
}
