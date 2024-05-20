using EduProfileAPI.DataAccessLayer;
using EduProfileAPI.Models.Class;
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

        public async Task<Class[]> GetAllClassesAsync()
        {
            IQueryable<Class> query = _context.Classes;
            return await query.ToArrayAsync();
        }

<<<<<<< Updated upstream
           
=======
        public async Task<Class> GetClassAsync(Guid classId)
        {
            return await _context.Classes
                                 .Where(c => c.ClassId == classId)
                                 .FirstOrDefaultAsync();
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

>>>>>>> Stashed changes
    }
}
