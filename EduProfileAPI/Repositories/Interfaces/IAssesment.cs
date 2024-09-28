using EduProfileAPI.Models;

namespace EduProfileAPI.Repositories.Interfaces
{
    public interface IAssesment
    {
        Task<IEnumerable<Assesment>> GetAllAssessmentsAsync();
        Task<bool> SaveChangesAsync();
        Task AddAssessmentAsync(Assesment assesment, Guid userId);
        Task UpdateAssessmentAsync(Assesment updatedAssesment, Assesment oldAssesment, Guid userId);
        Task DeleteAssessmentAsync(Assesment assesment, Guid userId);       
        Task<Assesment> GetAssessmentByIdAsync(Guid assessmentId);

    }
}
