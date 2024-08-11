using System;
using System.Threading.Tasks;
using EduProfileAPI.Models;

namespace EduProfileAPI.Repositories.Interfaces
{
    public interface IDisciplinaryTypeRepository
    {
        Task<DisciplinaryType[]> GetAllDisciplinaryTypesAsync();
        Task<DisciplinaryType> GetDisciplinaryTypeAsync(Guid disciplinaryTypeId);
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<bool> SaveChangesAsync();
    }
}
