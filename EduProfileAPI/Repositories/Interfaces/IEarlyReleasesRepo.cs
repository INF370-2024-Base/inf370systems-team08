using EduProfileAPI.Models;
using EduProfileAPI.ViewModels;

namespace EduProfileAPI.Repositories.Interfaces
{
    public interface IEarlyReleasesRepo
    {
        Task<EarlyReleases> CreateEarlyRelease(CreateEarlyReleaseVM model);
    }
}
