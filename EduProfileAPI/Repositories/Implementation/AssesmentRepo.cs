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

   
        public async Task<IEnumerable<Assesment>> GetAllAssessmentsAsync()
        {
            return await _context.Assesment.ToListAsync();
        }

        public async Task<Assesment> GetAssessmentByIdAsync(Guid assessmentId)
        {
            return await _context.Assesment.FindAsync(assessmentId);
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

    }
}
