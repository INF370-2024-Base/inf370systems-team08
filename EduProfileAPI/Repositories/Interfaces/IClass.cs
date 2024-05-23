using EduProfileAPI.Models.Class;

namespace EduProfileAPI.Repositories.Interfaces
{
    public interface IClass 
    {
        Task<Class[]> GetAllClassesAsync();

        

    }
}
