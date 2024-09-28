using EduProfileAPI.DataAccessLayer;
using EduProfileAPI.Models;
using EduProfileAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace EduProfileAPI.Repositories.Implementation
{
    public class DisciplinaryRepository : IDisciplinaryRepository
    {
        private readonly EduProfileDbContext _context;
        private readonly IAuditTrail _auditTrailRepository;

        public DisciplinaryRepository(EduProfileDbContext context, IAuditTrail auditTrailRepo)
        {
            _context = context;
            _auditTrailRepository = auditTrailRepo;
        }

        public async Task<List<Disciplinary>> GetAllDisciplinariesAsync()
        {
            return await _context.Disciplinary
                .FromSqlRaw("EXEC GetAllDisciplinaries")
                .ToListAsync();
        }

        public async Task<Disciplinary> GetDisciplinaryAsync(Guid disciplinaryId)
        {
            IQueryable<Disciplinary> query = _context.Disciplinary.Where(c => c.DisciplinaryId == disciplinaryId);
            return await query.FirstOrDefaultAsync();
        }

        public async Task AddDisciplinaryAsync(Disciplinary disciplinary, Guid userId)
        {
            _context.Add(disciplinary);

            var auditTrail = new AuditTrail
            {
                UserId = userId,
                Action = "CREATE",
                EntityName = "Disciplinary",
                AffectedEntityID = disciplinary.DisciplinaryId,
                NewValue = JsonConvert.SerializeObject(disciplinary), // Log the new disciplinary data
                TimeStamp = DateTime.UtcNow
            };

            await _auditTrailRepository.AddAuditTrailAsync(auditTrail);
        }


        public async Task UpdateDisciplinaryAsync(Disciplinary updatedDisciplinary, Disciplinary oldDisciplinary, Guid userId)
        {
            _context.Entry(oldDisciplinary).CurrentValues.SetValues(updatedDisciplinary);

            var auditTrail = new AuditTrail
            {
                UserId = userId,
                Action = "UPDATE",
                EntityName = "Disciplinary",
                AffectedEntityID = updatedDisciplinary.DisciplinaryId,
                OldValue = JsonConvert.SerializeObject(oldDisciplinary), // Log the old data
                NewValue = JsonConvert.SerializeObject(updatedDisciplinary), // Log the new data
                TimeStamp = DateTime.UtcNow
            };

            await _auditTrailRepository.AddAuditTrailAsync(auditTrail);
        }


        public async Task DeleteDisciplinaryAsync(Disciplinary disciplinary, Guid userId)
        {
            _context.Disciplinary.Remove(disciplinary);

            var auditTrail = new AuditTrail
            {
                UserId = userId,
                Action = "DELETE",
                EntityName = "Disciplinary",
                AffectedEntityID = disciplinary.DisciplinaryId,
                OldValue = JsonConvert.SerializeObject(disciplinary), // Log the old data
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

