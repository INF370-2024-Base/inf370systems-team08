using EduProfileAPI.Models;

namespace EduProfileAPI.Repositories.Interfaces
{
    public interface IClass
    {
        //Task<Class[]> GetAllClassesAsync();
        Task<List<Class>> GetAllClassesAsync();
        Task<bool> SaveChangesAsync();
        void Add<T>(T entity) where T: class;
        void Delete<T>(T entity) where T: class;
        Task<Class> GetClassAsync(Guid id);
    }
}
