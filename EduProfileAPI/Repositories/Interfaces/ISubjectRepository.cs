using EduProfileAPI.Models;
using EduProfileAPI.ViewModels;

namespace EduProfileAPI.Repositories.Interfaces
{
    public interface ISubjectRepository
    {
        Task<Subject[]> GetAllSubjectAsync();
        Task<SubjectViewModel> CreateSubjectAsync(SubjectViewModel model); // change to CreateSubjectViewModel later
        Task<SubjectViewModel> GetSubjectByIdAsync(Guid id);
        Task<SubjectViewModel> UpdateSubjectAsync(SubjectViewModel model);// change to UpdateSubjectViewModel later
        Task<bool> DeleteSubjectAsync(Guid id);
    }
}
