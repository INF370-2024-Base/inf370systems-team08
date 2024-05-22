using EduProfileAPI.Models;

namespace EduProfileAPI.Repositories.Interfaces
{
    public interface IEmployeeRepository
    {

        Task<Employee[]> GetAllEmployeesAsync();
        Task<bool> SaveChangesAsync();
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;


        Task<Employee> GetEmployeeAsync(int employeeId);
        Task GetEmployeeAsync(Guid employeeId);
    }
}

