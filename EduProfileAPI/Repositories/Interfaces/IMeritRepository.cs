using EduProfileAPI.Models;

namespace EduProfileAPI.Repositories.Interfaces
{
    public interface IMeritRepository
    {
        Task<List<Merit>> GetAllMeritsAsync();
        Task<bool> SaveChangesAsync();
        Task AddMeritAsync(Merit merit, Guid userId);
        Task UpdateMeritAsync(Merit updatedMerit, Merit oldMerit, Guid userId);
        Task DeleteMeritAsync(Merit merit, Guid userId);
        // Course
        Task<Merit> GetMeritAsync(Guid meritId);

    }
}
