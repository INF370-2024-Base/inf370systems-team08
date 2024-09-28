using EduProfileAPI.DataAccessLayer;
using EduProfileAPI.Models;
using EduProfileAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace EduProfileAPI.Repositories.Implementation
{
    public class StudentDocRepository : IStudentDocRepository
    {
        private readonly EduProfileDbContext _context;
        private readonly IAuditTrail _auditTrailRepository;

        public StudentDocRepository(EduProfileDbContext context, IAuditTrail auditTrailRepository)
        {
            _context = context;
            _auditTrailRepository = auditTrailRepository;
        }

        public async Task<StudentDoc[]> GetAllStudentDocsAsync()
        {
            IQueryable<StudentDoc> query = _context.StudentDocument;
            return await query.ToArrayAsync();
        }

        public async Task<StudentDoc> GetStudentDocAsync(Guid studentDocId)
        {
            IQueryable<StudentDoc> query = _context.StudentDocument.Where(c => c.StuDocumentId == studentDocId);
            return await query.FirstOrDefaultAsync();
        }

        public async Task AddStudentDocAsync(StudentDoc studentDoc, Guid userId)
        {
            _context.Add(studentDoc);

            var auditTrail = new AuditTrail
            {
                UserId = userId,
                Action = "CREATE",
                EntityName = "StudentDoc",
                AffectedEntityID = studentDoc.StuDocumentId,
                NewValue = JsonConvert.SerializeObject(studentDoc),
                TimeStamp = DateTime.UtcNow
            };

            await _auditTrailRepository.AddAuditTrailAsync(auditTrail);
        }

        public async Task UpdateStudentDocAsync(StudentDoc updatedDoc, StudentDoc oldDoc, Guid userId)
        {
            _context.Entry(oldDoc).CurrentValues.SetValues(updatedDoc);

            var auditTrail = new AuditTrail
            {
                UserId = userId,
                Action = "UPDATE",
                EntityName = "StudentDoc",
                AffectedEntityID = updatedDoc.StuDocumentId,
                OldValue = JsonConvert.SerializeObject(oldDoc),
                NewValue = JsonConvert.SerializeObject(updatedDoc),
                TimeStamp = DateTime.UtcNow
            };

            await _auditTrailRepository.AddAuditTrailAsync(auditTrail);
        }

        public async Task DeleteStudentDocAsync(StudentDoc studentDoc, Guid userId)
        {
            _context.StudentDocument.Remove(studentDoc);

            var auditTrail = new AuditTrail
            {
                UserId = userId,
                Action = "DELETE",
                EntityName = "StudentDoc",
                AffectedEntityID = studentDoc.StuDocumentId,
                OldValue = JsonConvert.SerializeObject(studentDoc),
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
