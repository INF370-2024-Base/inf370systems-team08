using EduProfileAPI.DataAccessLayer;
using EduProfileAPI.Models;
using EduProfileAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EduProfileAPI.Repositories.Implementation
{
    public class AssesmentRepo: IAssesment
    {
        private readonly EduProfileDbContext _context;
        public AssesmentRepo(EduProfileDbContext context)
        {
            _context = context;
        }

        public async Task<Assesment[]> GetAllAssesmentsAsync()
        {
            IQueryable<Assesment> query = _context.Assesment;
            return await query.ToArrayAsync();
        }

        public async Task<Assesment> GetAssesmentAsync(Guid assesmentId)
        {
            IQueryable<Assesment> query = _context.Assesment.Where(c => c.AssesmentId == assesmentId);
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
