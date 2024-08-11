using System;
using System.Linq;
using System.Threading.Tasks;
using EduProfileAPI.DataAccessLayer;
using EduProfileAPI.Models;
using EduProfileAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EduProfileAPI.Repositories.Implementation
{
    public class DisciplinaryTypeRepository : IDisciplinaryTypeRepository
    {
        private readonly EduProfileDbContext _context;

        public DisciplinaryTypeRepository(EduProfileDbContext context)
        {
            _context = context;
        }

        public async Task<DisciplinaryType[]> GetAllDisciplinaryTypesAsync()
        {
            IQueryable<DisciplinaryType> query = _context.DisciplinaryType;
            return await query.ToArrayAsync();
        }

        public async Task<DisciplinaryType> GetDisciplinaryTypeAsync(Guid disciplinaryTypeId)
        {
            IQueryable<DisciplinaryType> query = _context.DisciplinaryType.Where(d => d.DisciplinaryTypeId == disciplinaryTypeId);
            return await query.FirstOrDefaultAsync();
        }

        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
