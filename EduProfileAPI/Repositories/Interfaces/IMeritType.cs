using EduProfileAPI.Models;

namespace EduProfileAPI.Repositories.Interfaces
{
    public interface IMeritType
    {
        Task<MeritType[]> GetAllMeritTypesAsync();

        Task<MeritType> GetMeritTypeAsync(Guid meritTypeId);
    }
}
