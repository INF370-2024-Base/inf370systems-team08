using EduProfileAPI.DataAccessLayer;
using EduProfileAPI.Models;
using EduProfileAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EduProfileAPI.Repositories.Implementation
{
    public class ReportTypeRepo : IReportType
    {
        private readonly EduProfileDbContext _context;
        public ReportTypeRepo(EduProfileDbContext context)
        {
            _context = context;
        }

        public async Task<ReportType[]> GetAllReportTypesAsync()
        {
            IQueryable<ReportType> query = _context.ReportType;
            return await query.ToArrayAsync();
        }

        public async Task<ReportType> GetReportTypeAsync(Guid reportTypeId)
        {
            IQueryable<ReportType> query = _context.ReportType.Where(c => c.ReportTypeId == reportTypeId);
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
