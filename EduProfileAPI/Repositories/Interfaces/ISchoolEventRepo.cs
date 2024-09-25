using EduProfileAPI.Models;
using EduProfileAPI.ViewModels;

namespace EduProfileAPI.Repositories.Interfaces
{
    public interface ISchoolEventRepo
    {
        Task<SchoolEvent> CreateSchoolEvent(CreateSchoolEventViewModel model);
        Task<List<SchoolEvent>> GetAllSchoolEvents();
        Task<bool> SaveChangesAsync();
        void Delete<T>(T entity) where T : class;
        Task<SchoolEvent> GetSchoolEventAsync(Guid eventId);

    }

}
