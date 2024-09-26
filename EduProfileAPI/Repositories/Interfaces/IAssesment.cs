using EduProfileAPI.Models;

namespace EduProfileAPI.Repositories.Interfaces
{
    public interface IAssesment
    {
        Task<IEnumerable<Assesment>> GetAllAssessmentsAsync();
        Task<Assesment> GetAssessmentByIdAsync(Guid assessmentId);
        Task AddAssessmentAsync(Assesment assessment);
        Task UpdateAssessmentAsync(Assesment assessment);
        Task DeleteAssessmentAsync(Guid assessmentId);
    }
}
