using EduProfileAPI.Models;
using EduProfileAPI.ViewModels;

namespace EduProfileAPI.Repositories.Interfaces
{
    public interface IGradeRepository
    {
        Task<Grade[]> GetAllGradesAsync();
        Task<GradeViewModel> CreateGradeAsync(CreateGradeViewModel model);    
        Task<GradeViewModel> GetGradeByIdAsync(Guid id);
        Task<GradeViewModel> UpdateGradeAsync(UpdateGradeViewModel model);
        Task<bool> DeleteGradeAsync(Guid id);
    
    }
}