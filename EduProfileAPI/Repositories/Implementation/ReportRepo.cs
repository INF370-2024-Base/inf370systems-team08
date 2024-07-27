using EduProfileAPI.DataAccessLayer;
using EduProfileAPI.Models;
using EduProfileAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EduProfileAPI.Repositories.Implementation
{
    public class ReportRepo: IReport
    {
        private readonly EduProfileDbContext _context;
        public ReportRepo(EduProfileDbContext context)
        {
            _context = context;
        }

        public async Task<Report[]> GetAllReportsAsync()
        {
            IQueryable<Report> query = _context.Report;
            return await query.ToArrayAsync();
        }

        public async Task<Report> GetReportAsync(Guid reportId)
        {
            IQueryable<Report> query = _context.Report.Where(c => c.ReportId == reportId);
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
