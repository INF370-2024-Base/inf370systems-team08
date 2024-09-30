using EduProfileAPI.DataAccessLayer;
using EduProfileAPI.Models;
using EduProfileAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EduProfileAPI.Repositories.Implementation
{
    public class StudentDocumentTypeRepo : IStudentDocumentType
    {
        private readonly EduProfileDbContext _context;
        public StudentDocumentTypeRepo(EduProfileDbContext context)
        {
            _context = context;
        }

        public async Task<StudentDocumentType[]> GetAllDocTypesAsync()
        {
            IQueryable<StudentDocumentType> query = _context.StudentDocumentType;
            return await query.ToArrayAsync();
        }

        public async Task<StudentDocumentType> GetDocTypeAsync(Guid documentTypeId)
        {
            IQueryable<StudentDocumentType> query = _context.StudentDocumentType.Where(c => c.DocumentTypeId == documentTypeId);
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