using EduProfileAPI.Models;

namespace EduProfileAPI.Repositories.Interfaces
{
    public interface IStudentDocumentType
    {
        Task<StudentDocumentType[]> GetAllDocTypesAsync();
        Task<bool> SaveChangesAsync();
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;

        Task<StudentDocumentType> GetDocTypeAsync(Guid DocumentTypeId);
    }
}
