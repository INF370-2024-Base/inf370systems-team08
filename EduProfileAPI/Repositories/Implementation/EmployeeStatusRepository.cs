using EduProfileAPI.DataAccessLayer;
using EduProfileAPI.Models;
using EduProfileAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace EduProfileAPI.Repositories.Implementation
{
    public class EmployeeStatusRepository : IEmployeeStatusRepository
    {
        private readonly EduProfileDbContext _context;

        public EmployeeStatusRepository(EduProfileDbContext context)
        {
            _context = context;
        }

        public async Task<EmployeeStatus[]> GetAllEmployeeStatusesAsync()
        {
            IQueryable<EmployeeStatus> query = _context.EmployeeStatus;
            return await query.ToArrayAsync();
        }

        public async Task<EmployeeStatus> GetEmployeeStatusAsync(Guid employeeStatusId)
        {
            IQueryable<EmployeeStatus> query = _context.EmployeeStatus.Where(e => e.EmployeeStatusId == employeeStatusId);
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
