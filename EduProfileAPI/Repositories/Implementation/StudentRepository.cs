using EduProfileAPI.DataAccessLayer;
using EduProfileAPI.Models;
using EduProfileAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EduProfileAPI.Repositories.Implementation
{
    public class StudentRepository : IStudentRepository
    {
        private readonly EduProfileDbContext _context;
        public StudentRepository(EduProfileDbContext context)
        {
            _context = context;
        }

        public async Task<Student[]> GetAllStudentsAsync()
        {
            IQueryable<Student> query = _context.Student;
            return await query.ToArrayAsync();
        }

        public async Task<Student> GetStudentAsync(Guid studentId)
        {
            IQueryable<Student> query = _context.Student.Where(c => c.StudentId == studentId);
            return await query.FirstOrDefaultAsync();
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
    }
}
