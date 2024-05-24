using EduProfileAPI.Repositories.Interfaces;
using EduProfileAPI.DataAccessLayer;
using EduProfileAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EduProfileAPI.Repositories.Implementation
{
    public class EducationPhaseRepository: IEducationPhaseRepository
    {
        
        
            private readonly EduProfileDbContext _context;
            
            public EducationPhaseRepository(EduProfileDbContext context)
            {
                _context = context;
            }

             public async Task<IEnumerable<StudentEducationPhase>> GetAllEducationPhasesAsync()
             {
                return await _context.StudentEducationPhase.ToListAsync();
             }

            public async Task<StudentEducationPhase> GetEducationPhaseByIdAsync(Guid id)
            {
                return await _context.StudentEducationPhase.FirstOrDefaultAsync(e => e.StudentEducationPhaseId == id);

            }

            //public override int GetHashCode()
            //{
            //    return base.GetHashCode();
            //}

            //public override string? ToString()
            //{
            //    return base.ToString();
            //}
    }
    
}
