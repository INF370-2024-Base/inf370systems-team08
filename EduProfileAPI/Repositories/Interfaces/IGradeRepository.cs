using EduProfileAPI.Models;
using EduProfileAPI.ViewModels;

namespace EduProfileAPI.Repositories.Interfaces
{
    public interface IGradeRepository
    {
        Task<List<Grade>> GetAllGradesAsync();
        Task<GradeViewModel> CreateGradeAsync(CreateGradeViewModel model, Guid userId);    
        Task<GradeViewModel> GetGradeByIdAsync(Guid id);
        Task<GradeViewModel> UpdateGradeAsync(UpdateGradeViewModel model, Guid userId);
        Task<bool> DeleteGradeAsync(Guid id, Guid userId);
    }
}