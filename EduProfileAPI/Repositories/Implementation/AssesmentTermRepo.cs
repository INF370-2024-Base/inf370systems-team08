using EduProfileAPI.DataAccessLayer;
using EduProfileAPI.Models;
using EduProfileAPI.Repositories.Interfaces;
using EduProfileAPI.ViewModels;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sms77.Api.Library;
namespace EduProfileAPI.Repositories.Implementation
{
    public class AssesmentTermRepo : IAssesmentTerm
    {
        private readonly EduProfileDbContext _context;

        public AssesmentTermRepo(EduProfileDbContext context)
        {
            _context = context;
        }


        public async Task<AssesmentTerm[]> GetAllTermsAsync()
        {
                return await _context.AssesmentTerm.ToArrayAsync();
            
        }

        public async Task<AssesmentTerm> GetTermByIdAsync(Guid termId)
        {
            IQueryable<AssesmentTerm> query = _context.AssesmentTerm.Where(c => c.TermId == termId);

            return await query.FirstOrDefaultAsync();
        }


        public async Task<bool> AddAssesmentTermAsync(AssesmentTerm term)
        {
                _context.AssesmentTerm.Add(term);
                return await SaveChangesAsync();
        }

        public async Task<bool> UpdateAssesmentTermAsync(AssesmentTerm term)
        {
                var existingTerm = await GetTermByIdAsync(term.TermId);

                existingTerm.AssesmentId = term.AssesmentId;
                existingTerm.Term = term.Term;
                existingTerm.Weighting = term.Weighting;
                existingTerm.SubjectId = term.SubjectId;

                _context.AssesmentTerm.Update(existingTerm);
                return await SaveChangesAsync();

        }


        public async Task<bool> DeleteAssesmentTermAsync(Guid termId)
        {
                var term = await GetTermByIdAsync(termId);

                _context.AssesmentTerm.Remove(term);
                return await SaveChangesAsync();
        }


        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

    }
}
