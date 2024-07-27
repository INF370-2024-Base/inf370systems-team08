using EduProfileAPI.Models;

namespace EduProfileAPI.Repositories.Interfaces
{
    public interface IDisciplinaryRepository
    {
        Task<Disciplinary[]> GetAllDisciplinariesAsync();
        Task<bool> SaveChangesAsync();
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;

        // Course
        Task<Disciplinary> GetDisciplinaryAsync(Guid disciplinaryId);
    }
}
