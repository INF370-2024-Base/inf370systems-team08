using EduProfileAPI.Models;
using EduProfileAPI.ViewModels;

namespace EduProfileAPI.Repositories.Interfaces
{
    public interface ISubjectRepository
    {
        Task<Subject[]> GetAllSubjectAsync();
        Task<SubjectViewModel> CreateSubjectAsync(CreateSubjectViewModel model); 
        Task<SubjectViewModel> GetSubjectByIdAsync(Guid id);
        Task<SubjectViewModel> UpdateSubjectAsync(UpdateSubjectViewModel model);
        Task<bool> DeleteSubjectAsync(Guid id);
        Task<StudentSubject[]> GetAllStudentSubjectAsync();
    }
}
