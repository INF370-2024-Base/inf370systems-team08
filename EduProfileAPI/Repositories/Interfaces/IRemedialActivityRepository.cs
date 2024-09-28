using EduProfileAPI.Models;

namespace EduProfileAPI.Repositories.Interfaces
{
    public interface IRemedialActivityRepository
    {
        Task<List<RemedialActivity>> GetAllRemedialActivitiesAsync();
        Task<bool> SaveChangesAsync();
        Task AddRemedialActivityAsync(RemedialActivity activity, Guid userId);
        Task UpdateRemedialActivityAsync(RemedialActivity activity, Guid userId);
        Task<RemedialActivity> GetRemedialActivityAsync(Guid remedialActivityId);
        Task DeleteRemedialActivityAsync(RemedialActivity activity, Guid userId);

        Task<List<RemedialActivity>> GetActivitiesByFileId(Guid remFileId);
    }

}
