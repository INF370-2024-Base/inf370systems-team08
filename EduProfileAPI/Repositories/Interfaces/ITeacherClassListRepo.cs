using EduProfileAPI.Models;
using EduProfileAPI.ViewModels;

namespace EduProfileAPI.Repositories.Interfaces
{
    public interface ITeacherClassListRepo
    {
        Task<Student[]> StudentsInClass(Guid classId);
    }
}
