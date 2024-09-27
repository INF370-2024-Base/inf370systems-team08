using EduProfileAPI.Models;

namespace EduProfileAPI.Repositories.Interfaces
{
    public interface IAssesmentTerm
    {
        Task<AssesmentTerm[]> GetAllTermsAsync();
        Task<AssesmentTerm> GetTermByIdAsync(Guid termId);
        Task<bool> AddAssesmentTermAsync(AssesmentTerm term);
        Task<bool> UpdateAssesmentTermAsync(AssesmentTerm term);
        Task<bool> DeleteAssesmentTermAsync(Guid termId);
        Task<bool> SaveChangesAsync();
    }
}
