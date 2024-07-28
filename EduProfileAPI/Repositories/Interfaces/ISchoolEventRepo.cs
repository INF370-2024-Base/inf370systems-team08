using EduProfileAPI.Models;
using EduProfileAPI.ViewModels;

namespace EduProfileAPI.Repositories.Interfaces
{
    public interface ISchoolEventRepo
    {
        Task<SchoolEvent> CreateSchoolEvent(CreateSchoolEventViewModel model);
    }
}
