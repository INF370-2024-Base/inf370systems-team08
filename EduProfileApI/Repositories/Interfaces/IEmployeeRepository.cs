using EduProfileAPI.Models;
using System;
using System.Threading.Tasks;

namespace EduProfileAPI.Repositories.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<List<Employee>> GetAllEmployeesAsync();
        Task<Employee> GetEmployeeAsync(Guid employeeId); // Corrected the return type
        Task AddEmployeeAsync(Employee employee, Guid userId);
        Task UpdateEmployeeAsync(Employee updatedEmployee, Employee oldEmployee, Guid userId);
        Task DeleteEmployeeAsync(Employee employee, Guid userId);
        Task<bool> SaveChangesAsync();
    }
}

