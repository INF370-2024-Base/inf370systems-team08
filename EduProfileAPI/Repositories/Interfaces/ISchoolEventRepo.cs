using EduProfileAPI.Models;
using EduProfileAPI.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace EduProfileAPI.Repositories.Interfaces
{
    public interface ISchoolEventRepo
    {
        Task<SchoolEvent> CreateSchoolEventAsync(CreateSchoolEventViewModel model, Guid userId);
        Task<List<SchoolEvent>> GetAllSchoolEvents();
        Task<bool> SaveChangesAsync();
        Task UpdateSchoolEventAsync(SchoolEvent updatedSchoolEvent, Guid userId);
        Task DeleteSchoolEventAsync(SchoolEvent schoolEvent, Guid userId);


        Task<SchoolEvent> GetSchoolEventAsync(Guid eventId);

    }

}
