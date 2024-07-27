using EduProfileAPI.DataAccessLayer;
using EduProfileAPI.Models;
using EduProfileAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EduProfileAPI.Repositories.Implementation
{
    public class DisciplinaryRepository : IDisciplinaryRepository
    {
        private readonly EduProfileDbContext _context;
        public DisciplinaryRepository(EduProfileDbContext context)
        {
            _context = context;
        }

        public async Task<Disciplinary[]> GetAllDisciplinariesAsync()
        {
            IQueryable<Disciplinary> query = _context.Disciplinary;
            return await query.ToArrayAsync();
        }

        public async Task<Disciplinary> GetDisciplinaryAsync(Guid disciplinaryId)
        {
            IQueryable<Disciplinary> query = _context.Disciplinary.Where(c => c.DisciplinaryId == disciplinaryId);
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

