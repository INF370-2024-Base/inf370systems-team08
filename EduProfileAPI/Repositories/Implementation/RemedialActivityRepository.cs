using EduProfileAPI.DataAccessLayer;
using EduProfileAPI.Models;
using EduProfileAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;



namespace EduProfileAPI.Repositories.Implementation
{
    public class RemedialActivityRepository : IRemedialActivityRepository
    {
        private readonly EduProfileDbContext _context;

        public RemedialActivityRepository(EduProfileDbContext context)
        {
            _context = context;
        }

        public async Task<RemedialActivity[]> GetAllRemedialActivitiesAsync()
        {
            IQueryable<RemedialActivity> query = _context.RemedialActivity;
            return await query.ToArrayAsync();
        }

        public async Task<RemedialActivity> GetRemedialActivityAsync(Guid remedialActivityId)
        {
            IQueryable<RemedialActivity> query = _context.RemedialActivity.Where(c => c.RemActId == remedialActivityId);
            return await query.FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(RemedialActivity activity)
        {
            _context.RemedialActivity.Update(activity);
            await _context.SaveChangesAsync();

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

        public async Task<List<RemedialActivity>> GetActivitiesByFileId(Guid remFileId)
        {
            return await _context.RemedialActivity
                                 .Where(a => a.RemFileId == remFileId)
                                 .ToListAsync();
        }

    }

}
