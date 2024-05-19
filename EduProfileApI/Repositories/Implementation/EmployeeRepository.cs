using EduProfileAPI.DataAccessLayer;
using EduProfileAPI.Models;
using EduProfileAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EduProfileAPI.Repositories.Implementation
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly EduProfileDbContext _context;
        public EmployeeRepository(EduProfileDbContext context)
        {
            _context = context;
        }

        public void Add<T>(T entity) where T : class
        {
            throw new NotImplementedException();
        }

        public void Delete<T>(T entity) where T : class
        {
            throw new NotImplementedException();
        }

        public Task<Employee[]> GetAllEmployeesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Employee> GetEmployeeAsync(int employeeId)
        {
            throw new NotImplementedException();
        }

        public Task GetEmployeeAsync(Guid employeeId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SaveChangesAsync()
        {
            throw new NotImplementedException();
        }

        Task<Employee[]> IEmployeeRepository.GetAllEmployeesAsync()
        {
            throw new NotImplementedException();
        }

        Task<Employee> IEmployeeRepository.GetEmployeeAsync(int employeeId)
        {
            throw new NotImplementedException();
        }
    }
}
