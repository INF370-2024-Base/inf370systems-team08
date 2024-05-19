using EduProfileAPI.Models;

namespace EduProfileAPI.Repositories.Interfaces
{
    public interface IEmployeeRepository
    {

        Task<Merit[]> GetAllEmployeesAsync();
        Task<bool> SaveChangesAsync();
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;


        Task<Merit> GetEmployeeAsync(int employeeId);
        Task GetEmployeeAsync(Guid employeeId);
    }
}
