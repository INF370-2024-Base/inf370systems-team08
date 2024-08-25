using EduProfileAPI.Models;
using EduProfileAPI.ViewModels;

namespace EduProfileAPI.Repositories.Interfaces
{
    public interface IStudentRepository
    {
        Task<Student[]> GetAllStudentsAsync();
        Task<bool> SaveChangesAsync();
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<Student> GetStudentAsync(Guid studentId);
        Task<Parent[]> GetAllParentsAsync();
        Task<Parent> GetParentAsync(Guid parentId);
        Task<Student[]> GetStudentsByParentIdAsync(Guid parentId);
        Task<List<ParentEmailVM>> GetAllParentEmailsAsync();
    }
}
