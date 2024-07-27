using EduProfileAPI.Models;

namespace EduProfileAPI.Repositories.Interfaces
{
    public interface IAssesmentMark
    {
        Task<AssesmentMark[]> GetAllAssesmentMarksAsync();
        Task<bool> SaveChangesAsync();
        void Add<T>(T entity) where T : class;
        Task<AssesmentMark> GetAssesmentMarkAsync(Guid studentId, Guid assesmentId);
    }
}
