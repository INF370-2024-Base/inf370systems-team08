using EduProfileAPI.Models;

namespace EduProfileAPI.Repositories.Interfaces
{
    public interface IAssesmentMark
    {
        Task<AssesmentMark[]> GetAllAssesmentMarksAsync();
        Task<bool> SaveChangesAsync();
        Task AddAssesmentMarkAsync(AssesmentMark assesmentMark, Guid userId);
        Task UpdateAssesmentMarkAsync(AssesmentMark updatedMark, AssesmentMark oldMark, Guid userId);
        Task<AssesmentMark> GetAssesmentMarkAsync(Guid studentId, Guid assesmentId);
    }
}
