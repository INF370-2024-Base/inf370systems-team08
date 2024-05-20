using EduProfileAPI.Models;

namespace EduProfileAPI.Repositories.Interfaces
{
    public interface IStudentDocRepository
    {
        Task<StudentDoc[]> GetAllStudentDocsAsync();
        Task<bool> SaveChangesAsync();
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<StudentDoc> GetStudentDocAsync(Guid studentDocId);
    }
}
