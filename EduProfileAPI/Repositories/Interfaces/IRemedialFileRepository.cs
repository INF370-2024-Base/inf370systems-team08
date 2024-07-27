using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EduProfileAPI.Models;

namespace EduProfileAPI.Repositories.Interfaces
{
    public interface IRemedialFileRepository
    {
        Task<RemedialFile[]> GetAllRemedialFileAsync(); 
        Task<bool> SaveChangesAsync();
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<RemedialFile> GetRemedialFileAsync(Guid id);

    }
}
