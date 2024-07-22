using System;
using System.Linq;
using System.Threading.Tasks;
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

            // IQueryable<Class> query = _context.Class;
            var query = _context.RemedialActivity;

            return await query.ToArrayAsync();
        }

        public async Task<RemedialActivity> GetRemedialActivityByIdAsync(Guid remActId)
        {
            IQueryable<RemedialActivity> query = _context.RemedialActivity.Where(c => c.RemActId == remActId);

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
