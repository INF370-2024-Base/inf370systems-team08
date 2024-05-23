using EduProfileAPI.DataAccessLayer;
using EduProfileAPI.Models.Class;
using EduProfileAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EduProfileAPI.Repositories.Implementation
{
    public class ClassRepository: IClass
    {
        private readonly EduProfileDbContext _context;

        public ClassRepository(EduProfileDbContext context)
        {
            _context = context;
        }

        public async Task<Class[]> GetAllClassesAsync()
        {
            IQueryable<Class> query = _context.Classes;
            return await query.ToArrayAsync();
        }

           
    }
}
