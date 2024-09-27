using EduProfileAPI.Models;

namespace EduProfileAPI.Repositories.Interfaces
{
    public interface IClass
    {
        Task<List<Class>> GetAllClassesAsync();
        Task<bool> SaveChangesAsync();
        Task AddClassAsync(Class classEntity, Guid userId);
        Task UpdateClassAsync(Class existingClass, Class updatedClass, Guid userId);
        Task DeleteClassAsync(Class classEntity, Guid userId);
        Task<Class> GetClassAsync(Guid id);
    }
}
