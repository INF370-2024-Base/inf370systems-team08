using EduProfileAPI.DataAccessLayer;
using EduProfileAPI.Models;
using EduProfileAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EduProfileAPI.Repositories.Implementation
{
    public class MeritRepository : IMeritRepository
    {
        private readonly EduProfileDbContext _context;
        public MeritRepository(EduProfileDbContext context)
        {
            _context = context;
        }

        // Call the stored procedure to retrieve all merits
        public async Task<List<Merit>> GetAllMeritsAsync()
        {
            return await _context.Merit
                .FromSqlRaw("EXEC GetAllMerits")
                .ToListAsync();
        }

        public async Task<Merit> GetMeritAsync(Guid meritId)
        {
            IQueryable<Merit> query = _context.Merit.Where(c => c.MeritId == meritId);
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
