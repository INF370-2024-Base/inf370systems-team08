using EduProfileAPI.ViewModels;

namespace EduProfileAPI.Repositories.Interfaces
{
    public interface IAssignTeacherToClassRepository
    {
        Task<AssignTeacherToClassViewModel> AssignTeacherToClassAsync(AssignTeacherToClassViewModel model);
    }
}
