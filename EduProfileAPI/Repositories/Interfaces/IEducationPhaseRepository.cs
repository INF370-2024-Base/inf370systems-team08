using EduProfileAPI.Models;

namespace EduProfileAPI.Repositories.Interfaces
{
    public interface IEducationPhaseRepository
    {
        Task<StudentEducationPhase> GetEducationPhaseByIdAsync(Guid id);
        Task<IEnumerable<StudentEducationPhase>> GetAllEducationPhasesAsync();
    }
}
