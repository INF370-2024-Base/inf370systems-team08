using EduProfileAPI.DataAccessLayer;
using EduProfileAPI.Models;
using EduProfileAPI.Repositories.Interfaces;
using EduProfileAPI.ViewModels;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace EduProfileAPI.Repositories.Implementation
{
    public class StudentRepository : IStudentRepository
    {
        private readonly EduProfileDbContext _context;
        private readonly IAuditTrail _auditTrailRepository;

        public StudentRepository(EduProfileDbContext context, IAuditTrail auditTrailRepository)
        {
            _context = context;
            _auditTrailRepository = auditTrailRepository;
        }

        public async Task<List<Student>> GetAllStudentsAsync()
        {
            return await _context.Student.FromSqlRaw("EXEC GetAllStudents").ToListAsync();
        }

        public async Task<Student> GetStudentAsync(Guid studentId)
        {
            IQueryable<Student> query = _context.Student.Where(c => c.StudentId == studentId);
            return await query.FirstOrDefaultAsync();
        }

        public async Task AddStudentAsync(Student student, Guid userId)
        {
            _context.Student.Add(student);

            var auditTrail = new AuditTrail
            {
                UserId = userId,
                Action = "CREATE",
                EntityName = "Student",
                AffectedEntityID = student.StudentId,
                NewValue = JsonConvert.SerializeObject(student),
                TimeStamp = DateTime.UtcNow
            };

            _context.AuditTrail.Add(auditTrail);
        }

        // Update existing student with audit trail logging
        public async Task UpdateStudentAsync(Student updatedStudent, Student oldStudent, Guid userId)
        {
            _context.Entry(oldStudent).CurrentValues.SetValues(updatedStudent);

            var auditTrail = new AuditTrail
            {
                UserId = userId,
                Action = "UPDATE",
                EntityName = "Student",
                AffectedEntityID = updatedStudent.StudentId,
                OldValue = JsonConvert.SerializeObject(oldStudent),
                NewValue = JsonConvert.SerializeObject(updatedStudent),
                TimeStamp = DateTime.UtcNow
            };

            _context.AuditTrail.Add(auditTrail);
        }

        // Delete student with audit trail logging
        public async Task DeleteStudentAsync(Student student, Guid userId)
        {
            _context.Student.Remove(student);

            var auditTrail = new AuditTrail
            {
                UserId = userId,
                Action = "DELETE",
                EntityName = "Student",
                AffectedEntityID = student.StudentId,
                OldValue = JsonConvert.SerializeObject(student),
                TimeStamp = DateTime.UtcNow
            };

            _context.AuditTrail.Add(auditTrail);
        }

        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }
        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Parent[]> GetAllParentsAsync()
        {
            IQueryable<Parent> query =_context.Parent;
            return await query.ToArrayAsync();
        }

        public async Task<Parent> GetParentAsync(Guid parentId)
        {
            IQueryable<Parent> query = _context.Parent.Where(c => c.ParentId == parentId);
            return await query.FirstOrDefaultAsync();
        }

        public async Task<Student[]> GetStudentsByParentIdAsync(Guid parentId)
        {
            IQueryable<Student> query = _context.Student.Where(s => s.ParentId == parentId);
            return await query.ToArrayAsync();
        }

        public async Task<List<ParentEmailVM>> GetAllParentEmailsAsync()
        {
            return await _context.Parent.Select(p => new ParentEmailVM
            {
                ParentId = p.ParentId,
                Parent1Email = p.Parent1Email,
                Parent2Email = p.Parent2Email
            }).ToListAsync();
        }

        public async Task<string> GetRandomParentIdAsync()
        {
            var parentIds = await _context.Parent.Select(p => p.ParentId).ToListAsync();

            Random random = new Random();
            int index = random.Next(parentIds.Count);
            return parentIds[index].ToString();
        }
    }

}
