using EduProfileAPI.Models;
using EduProfileAPI.ViewModels;

namespace EduProfileAPI.Repositories.Interfaces
{
    public interface IEarlyReleasesRepo
    {
        Task<EarlyReleases> CreateEarlyRelease(CreateEarlyReleaseVM model, Guid userId);
        Task<List<EarlyReleases>> GetEarlyReleasesByStudentId(Guid studentId);
    }
}
