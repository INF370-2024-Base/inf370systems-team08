using EduProfileAPI.Models;

namespace EduProfileAPI.Repositories.Interfaces
{
    public interface IStudentIncidentRepository
    {
        Task<IEnumerable<StudentIncident>> GetAllAsync();
        Task<StudentIncident?> GetByIdAsync(Guid? id);
        Task<StudentIncident> AddAsync(StudentIncident studentIncident);
        Task<StudentIncident> UpdateAsync(StudentIncident studentIncident);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
        Task<IEnumerable<IncidentType>> GetAllTypesAsync();
        Task<bool> SaveChangesAsync();
    }
}
