using EduProfileAPI.Models;

namespace EduProfileAPI.Repositories.Interfaces
{
    public interface IGradeRepository
    {
        Task<Grade[]> GetAllGradesAsync();
    }
}