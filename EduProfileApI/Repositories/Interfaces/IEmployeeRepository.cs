using EduProfileAPI.Models;
using System;
using System.Threading.Tasks;

namespace EduProfileAPI.Repositories.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<List<Employee>> GetAllEmployeesAsync();
        Task<Employee> GetEmployeeAsync(Guid employeeId); // Corrected the return type
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<bool> SaveChangesAsync();
    }
}

