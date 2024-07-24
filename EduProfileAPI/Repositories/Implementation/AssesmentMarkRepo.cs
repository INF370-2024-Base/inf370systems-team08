using EduProfileAPI.DataAccessLayer;
using EduProfileAPI.Models;
using EduProfileAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EduProfileAPI.Repositories.Implementation
{
    public class AssesmentMarkRepo: IAssesmentMark
    {
        private readonly EduProfileDbContext _context;
        public AssesmentMarkRepo(EduProfileDbContext context)
        {
            _context = context;
        }

        public async Task<AssesmentMark[]> GetAllAssesmentMarksAsync()
        {
            IQueryable<AssesmentMark> query = _context.AssesmentMark;
            return await query.ToArrayAsync();
        }

        public async Task<AssesmentMark> GetAssesmentMarkAsync(Guid studentId, Guid assesmentId)
        {
            IQueryable<AssesmentMark> query = _context.AssesmentMark.Where(c => c.StudentId  == studentId && c.AssementId == assesmentId);
            return await query.FirstOrDefaultAsync();
        }

        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }


        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
