using EduProfileAPI.DataAccessLayer;
using EduProfileAPI.Models;
using EduProfileAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace EduProfileAPI.Repositories.Implementation
{
    public class GradeRepository : IGradeRepository
    {
        private readonly EduProfileDbContext _context;
        public GradeRepository(EduProfileDbContext context)
        {
            _context = context;
        }

        public async Task<Grade[]> GetAllGradesAsync()
        {
            IQueryable<Grade> query = _context.Grades;
            return await query.ToArrayAsync();
        }
    }
}