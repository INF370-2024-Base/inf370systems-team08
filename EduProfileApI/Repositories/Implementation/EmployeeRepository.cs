using EduProfileAPI.DataAccessLayer;
using EduProfileAPI.Models;
using EduProfileAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using EduProfileAPI.ViewModels;
using Newtonsoft.Json;

namespace EduProfileAPI.Repositories.Implementation
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly EduProfileDbContext _context;
        private readonly IAuditTrail _auditTrailRepository;  // Add audit trail repository

        public EmployeeRepository (EduProfileDbContext context, IAuditTrail auditTrailRepository)
        {
            _context = context;
            _auditTrailRepository = auditTrailRepository;
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

        // Add new employee with audit trail logging
        public async Task AddEmployeeAsync(Employee employee, Guid userId)
        {
            _context.Employee.Add(employee);

            var auditTrail = new AuditTrail
            {
                UserId = userId,
                Action = "CREATE",
                EntityName = "Employee",
                AffectedEntityID = employee.EmployeeId,
                NewValue = JsonConvert.SerializeObject(employee),
                TimeStamp = DateTime.UtcNow
            };

            _context.AuditTrail.Add(auditTrail);
        }

        // Update existing employee with audit trail logging
        public async Task UpdateEmployeeAsync(Employee updatedEmployee, Employee oldEmployee, Guid userId)
        {
            _context.Entry(oldEmployee).CurrentValues.SetValues(updatedEmployee);

            var auditTrail = new AuditTrail
            {
                UserId = userId,
                Action = "UPDATE",
                EntityName = "Employee",
                AffectedEntityID = updatedEmployee.EmployeeId,
                OldValue = JsonConvert.SerializeObject(oldEmployee),
                NewValue = JsonConvert.SerializeObject(updatedEmployee),
                TimeStamp = DateTime.UtcNow
            };

            _context.AuditTrail.Add(auditTrail);
        }

        // Delete employee with audit trail logging
        public async Task DeleteEmployeeAsync(Employee employee, Guid userId)
        {
            _context.Employee.Remove(employee);

            var auditTrail = new AuditTrail
            {
                UserId = userId,
                Action = "DELETE",
                EntityName = "Employee",
                AffectedEntityID = employee.EmployeeId,
                OldValue = JsonConvert.SerializeObject(employee),
                TimeStamp = DateTime.UtcNow
            };

            _context.AuditTrail.Add(auditTrail);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
