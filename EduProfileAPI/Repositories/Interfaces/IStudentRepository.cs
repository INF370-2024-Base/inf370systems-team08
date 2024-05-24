using EduProfileAPI.Models;

namespace EduProfileAPI.Repositories.Interfaces
{
    public interface IStudentRepository
    {
        Task<Student[]> GetAllStudentsAsync();
        Task<bool> SaveChangesAsync();
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;

        // Course
        Task<Student> GetStudentAsync(Guid studentId);
    }
}
