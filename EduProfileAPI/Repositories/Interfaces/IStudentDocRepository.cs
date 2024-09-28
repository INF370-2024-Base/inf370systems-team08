using EduProfileAPI.Models;

namespace EduProfileAPI.Repositories.Interfaces
{
    public interface IStudentDocRepository
    {
        Task<StudentDoc[]> GetAllStudentDocsAsync();
        Task<bool> SaveChangesAsync();
        Task AddStudentDocAsync(StudentDoc studentDoc, Guid userId);
        Task UpdateStudentDocAsync(StudentDoc updatedDoc, StudentDoc oldDoc, Guid userId);
        Task DeleteStudentDocAsync(StudentDoc studentDoc, Guid userId);
        Task<StudentDoc> GetStudentDocAsync(Guid studentDocId);
    }
}
