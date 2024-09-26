using EduProfileAPI.DataAccessLayer;
using EduProfileAPI.Models;
using EduProfileAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using EduProfileAPI.ViewModels;

namespace EduProfileAPI.Repositories.Implementation
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly EduProfileDbContext _context;

        public EmployeeRepository (EduProfileDbContext context)
        {
            _context = context;

        }

        // Call the stored procedure to retrieve all employees
        public async Task<List<Employee>> GetAllEmployeesAsync()
        {
            return await _context.Employee
                .FromSqlRaw("EXEC GetAllEmployees")
                .ToListAsync();
        }

        public async Task<Employee> GetEmployeeAsync(Guid employeeId)
        {
            IQueryable<Employee> query = _context.Employee.Where(e => e.EmployeeId == employeeId);
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
