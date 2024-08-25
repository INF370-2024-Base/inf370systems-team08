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
        Task<IncidentType?> GetByTypeIdAsync(Guid? id);
        Task<IEnumerable<IncidentType>> GetAllTypesAsync();
        Task<IEnumerable<IncidentType>> AddIncidentAsync(IncidentType incidentType);
        Task<IncidentType> UpdateIncidentType(IncidentType incidentType);
        Task<bool> DeleteTypeAsync(Guid id);
        Task<bool> ExistTypeAsync(Guid id);


        Task<bool> SaveChangesAsync();
        Task<IEnumerable<IncidentType>> AddIncidentAsync(IncidentType incidentType);
    }
}
