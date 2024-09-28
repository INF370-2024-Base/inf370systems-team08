using EduProfileAPI.Models;
using EduProfileAPI.ViewModels;

namespace EduProfileAPI.Repositories.Interfaces
{
    public interface IStudentRepository
    {
        Task<List<Student>> GetAllStudentsAsync();
        Task<bool> SaveChangesAsync();
        Task AddStudentAsync(Student student, Guid userId);
        Task UpdateStudentAsync(Student updatedStudent, Student oldStudent, Guid userId);

        Task DeleteStudentAsync(Student student, Guid userId);
        void Delete<T>(T entity) where T : class;
        void Add<T>(T entity) where T : class;
        Task<Student> GetStudentAsync(Guid studentId);
        Task<Parent[]> GetAllParentsAsync();
        Task<Parent> GetParentAsync(Guid parentId);
        Task<Student[]> GetStudentsByParentIdAsync(Guid parentId);
        Task<List<ParentEmailVM>> GetAllParentEmailsAsync();
        Task<string> GetRandomParentIdAsync();
    }
}
