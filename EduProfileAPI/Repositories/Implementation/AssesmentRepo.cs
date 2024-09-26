using EduProfileAPI.DataAccessLayer;
using EduProfileAPI.Models;
using EduProfileAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
namespace EduProfileAPI.Repositories.Implementation
{
    public class AssesmentRepo: IAssesment
    {
        private readonly EduProfileDbContext _context;

        public AssesmentRepo(EduProfileDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Assesment>> GetAllAssessmentsAsync()
        {
            return await _context.Assesment.ToListAsync();
        }

        public async Task<Assesment> GetAssessmentByIdAsync(Guid assessmentId)
        {
            return await _context.Assesment.FindAsync(assessmentId);
        }

        public async Task AddAssessmentAsync(Assesment assessment)
        {
            await _context.Assesment.AddAsync(assessment);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAssessmentAsync(Assesment assessment)
        {
            _context.Assesment.Update(assessment);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAssessmentAsync(Guid assessmentId)
        {
            var assessment = await _context.Assesment.FindAsync(assessmentId);
            if (assessment != null)
            {
                _context.Assesment.Remove(assessment);
                await _context.SaveChangesAsync();
            }
        }



    }
}
