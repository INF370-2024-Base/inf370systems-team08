using EduProfileAPI.DataAccessLayer;
using EduProfileAPI.Models;
using EduProfileAPI.Repositories.Interfaces;
using EduProfileAPI.ViewModels;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace EduProfileAPI.Repositories.Implementation
{
    public class EarlyReleasesRepo : IEarlyReleasesRepo
    {
        private readonly EduProfileDbContext _context;
        private readonly IAuditTrail _auditTrailRepo; // Inject the audit trail repository

        public EarlyReleasesRepo( EduProfileDbContext context, IAuditTrail auditTrailRepo)
        {
            _context = context;
            _auditTrailRepo = auditTrailRepo;
        }

        public async Task<EarlyReleases> CreateEarlyRelease(CreateEarlyReleaseVM model, Guid userId)
        {
            var student = await _context.Student.FirstOrDefaultAsync(s => s.StudentId == model.StudentId);

            if (student == null)
                return null;

            var parent = await _context.Parent.FirstOrDefaultAsync(p => p.ParentId == student.ParentId);

            if (parent == null)
                return null;

            var earlyRelease = new EarlyReleases
            {
                EarlyRelId = Guid.NewGuid(),
                StudentId = model.StudentId,
                ParentId = parent.ParentId,
                EmployeeId = model.EmployeeId,
                DateOfEarlyRelease = model.DateOfEarlyRelease,
                ReasonForEarlyRelease = model.ReasonForEarlyRelease,
                SignerRelationship = model.SignerRelationship,
                SignerName = model.SignerName,
                SignerIDNumber = model.SignerIDNumber
            };

            _context.EarlyReleases.Add(earlyRelease);
            await _context.SaveChangesAsync();

            // Log the creation in the audit trail
            var auditTrail = new AuditTrail
            {
                UserId = userId,  // The user who performed the action
                Action = "CREATE",
                EntityName = "EarlyReleases",
                AffectedEntityID = earlyRelease.EarlyRelId,
                NewValue = JsonConvert.SerializeObject(earlyRelease),
                TimeStamp = DateTime.UtcNow
            };

            await _auditTrailRepo.AddAuditTrailAsync(auditTrail);

            return earlyRelease;
        }


        public async Task<List<EarlyReleases>> GetEarlyReleasesByStudentId(Guid studentId)
        {
            var earlyReleases = await _context.EarlyReleases
                .Where(er => er.StudentId == studentId)
                .ToListAsync();

            return earlyReleases;
        }

    }
}
