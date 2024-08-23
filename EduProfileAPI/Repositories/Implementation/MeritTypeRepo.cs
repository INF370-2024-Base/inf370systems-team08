using EduProfileAPI.DataAccessLayer;
using EduProfileAPI.Models;
using EduProfileAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EduProfileAPI.Repositories.Implementation
{
    public class MeritTypeRepo: IMeritType
    {
        private readonly EduProfileDbContext _context;
        public MeritTypeRepo(EduProfileDbContext context)
        {
            _context = context;
        }

        public async Task<MeritType[]> GetAllMeritTypesAsync()
        {
            IQueryable<MeritType> query = _context.MeritType;
            return await query.ToArrayAsync();
        }

        public async Task<MeritType> GetMeritTypeAsync(Guid meritTypeId)
        {
            IQueryable<MeritType> query = _context.MeritType.Where(c => c.MeritTypeId == meritTypeId);
            return await query.FirstOrDefaultAsync();
        }
    }
}
