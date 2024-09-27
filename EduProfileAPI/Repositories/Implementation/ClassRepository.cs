using System;
using System.Linq;
using System.Threading.Tasks;
using EduProfileAPI.DataAccessLayer;
using EduProfileAPI.Models;
using EduProfileAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;


namespace EduProfileAPI.Repositories.Implementation
{
    public class ClassRepository: IClass
    {
        private readonly EduProfileDbContext _context;
        private readonly IAuditTrail _auditTrailRepository; // Inject the audit trail repository


        public ClassRepository(EduProfileDbContext context, IAuditTrail auditTrailRepository)
        {
            _context = context;
            _auditTrailRepository = auditTrailRepository;
        }

        public async Task<List<Class>> GetAllClassesAsync()
        {
            return await _context.Class
                .FromSqlRaw("EXEC GetAllClasses")
                .ToListAsync();
        }

        public async Task<Class> GetClassAsync(Guid classId)
        {
            IQueryable<Class> query = _context.Class.Where(c => c.ClassId == classId);
                                                    
            return await query.FirstOrDefaultAsync();
        }

        // Add a new class and log the action with userId
        public async Task AddClassAsync(Class classEntity, Guid userId)
        {
            _context.Add(classEntity);

            // Log the audit trail for the addition
            var auditTrail = new AuditTrail
            {
                UserId = userId,
                Action = "CREATE",
                EntityName = "Class",
                AffectedEntityID = classEntity.ClassId,
                NewValue = JsonConvert.SerializeObject(classEntity),
                TimeStamp = DateTime.UtcNow
            };

            _context.AuditTrail.Add(auditTrail);

            await SaveChangesAsync();
        }

        // Update an existing class and log the action with userId
        public async Task UpdateClassAsync(Class existingClass, Class updatedClass, Guid userId)
        {
            _context.Entry(existingClass).CurrentValues.SetValues(updatedClass);

            // Log the audit trail for the update
            var auditTrail = new AuditTrail
            {
                UserId = userId,
                Action = "UPDATE",
                EntityName = "Class",
                AffectedEntityID = updatedClass.ClassId,
                OldValue = JsonConvert.SerializeObject(existingClass),
                NewValue = JsonConvert.SerializeObject(updatedClass),
                TimeStamp = DateTime.UtcNow
            };

            _context.AuditTrail.Add(auditTrail);

            await SaveChangesAsync();
        }

        // Delete a class and log the action with userId
        public async Task DeleteClassAsync(Class classEntity, Guid userId)
        {
            _context.Class.Remove(classEntity);

            // Log the audit trail for the deletion
            var auditTrail = new AuditTrail
            {
                UserId = userId,
                Action = "DELETE",
                EntityName = "Class",
                AffectedEntityID = classEntity.ClassId,
                OldValue = JsonConvert.SerializeObject(classEntity),
                TimeStamp = DateTime.UtcNow
            };

            _context.AuditTrail.Add(auditTrail);

            await SaveChangesAsync();
        }


        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

    }
}
