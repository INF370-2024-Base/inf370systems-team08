using EduProfileAPI.ViewModels;

namespace EduProfileAPI.Repositories.Interfaces
{
    public interface IContactStudentParent
    {
        Task<ContactStudentParentViewModel> GetParentDetailsByStudentId(Guid studentId);
        Task<(bool, string)> SendMessageToParent(ContactStudentParentViewModel model);
    }
}
