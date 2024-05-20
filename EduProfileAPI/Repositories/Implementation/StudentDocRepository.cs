using EduProfileAPI.DataAccessLayer;
using EduProfileAPI.Models;
using EduProfileAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EduProfileAPI.Repositories.Implementation
{
    public class StudentDocRepository : IStudentDocRepository
    {
        private readonly EduProfileDbContext _context;
        public StudentDocRepository(EduProfileDbContext context)
        {
            _context = context;
        }

        public async Task<StudentDoc[]> GetAllStudentDocsAsync()
        {
            IQueryable<StudentDoc> query = _context.StudentDocs;
            return await query.ToArrayAsync();
        }

        public async Task<StudentDoc> GetStudentDocAsync(Guid studentDocId)
        {
            IQueryable<StudentDoc> query = _context.StudentDocs.Where(c => c.StuDocumentId == studentDocId);
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
    }
}
