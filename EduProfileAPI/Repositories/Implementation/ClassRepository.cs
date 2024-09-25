using System;
using System.Linq;
using System.Threading.Tasks;
using EduProfileAPI.DataAccessLayer;
using EduProfileAPI.Models;
using EduProfileAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace EduProfileAPI.Repositories.Implementation
{
    public class ClassRepository: IClass
    {
        private readonly EduProfileDbContext _context;
        
        public ClassRepository(EduProfileDbContext context)
        {
            _context = context;
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
