using EduProfileAPI.DataAccessLayer;
using EduProfileAPI.Models;
using EduProfileAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EduProfileAPI.Repositories.Implementation
{
    public class RemedialFileRepository: IRemedialFileRepository
    {
        private readonly EduProfileDbContext _context;

        public RemedialFileRepository(EduProfileDbContext context)
        {
            _context = context;
        }

        public async Task<RemedialFile[]> GetAllRemedialFileAsync()
        {

            // IQueryable<Class> query = _context.Class;
            var query = _context.RemedialFile;

            return await query.ToArrayAsync();
        }

        public async Task<RemedialFile> GetRemedialFileAsync(Guid remFileId)
        {
            IQueryable<RemedialFile> query = _context.RemedialFile.Where(c => c.RemFileId == remFileId);

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
