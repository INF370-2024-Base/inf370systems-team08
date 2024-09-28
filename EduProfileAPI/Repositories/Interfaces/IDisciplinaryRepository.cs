using EduProfileAPI.Models;

namespace EduProfileAPI.Repositories.Interfaces
{
    public interface IDisciplinaryRepository
    {
        Task<List<Disciplinary>> GetAllDisciplinariesAsync();
        Task<bool> SaveChangesAsync();
        Task AddDisciplinaryAsync(Disciplinary disciplinary, Guid userId);
        Task UpdateDisciplinaryAsync(Disciplinary updatedDisciplinary, Disciplinary oldDisciplinary, Guid userId);

        Task DeleteDisciplinaryAsync(Disciplinary disciplinary, Guid userId);

        // Course
        Task<Disciplinary> GetDisciplinaryAsync(Guid disciplinaryId);
    }
}
