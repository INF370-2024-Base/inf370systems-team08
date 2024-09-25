using EduProfileAPI.Models;

namespace EduProfileAPI.Repositories.Interfaces
{
    public interface IAssesment
    {
        Task<Assesment[]> GetAllAssesmentsAsync();
        Task<bool> SaveChangesAsync();
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<Assesment> GetAssesmentAsync(Guid assesmentId);
        Task<Assesment[]> GetAssessmentsByTermAsync(int term);
    }
}
