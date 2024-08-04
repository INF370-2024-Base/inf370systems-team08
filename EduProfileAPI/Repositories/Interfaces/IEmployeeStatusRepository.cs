using EduProfileAPI.Models;
using System;
using System.Threading.Tasks;

namespace EduProfileAPI.Repositories.Interfaces
{
    public interface IEmployeeStatusRepository
    {
        Task<EmployeeStatus[]> GetAllEmployeeStatusesAsync();
        Task<EmployeeStatus> GetEmployeeStatusAsync(Guid employeeStatusId);
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<bool> SaveChangesAsync();
    }
}
