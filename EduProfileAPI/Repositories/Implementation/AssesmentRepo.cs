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
        private readonly ILogger<AssesmentRepo> _logger;
        public AssesmentRepo(EduProfileDbContext context, ILogger<AssesmentRepo> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Assesment[]> GetAllAssesmentsAsync()
        {
            try
            {
                // Fetch assessments, handle null checks safely
                var assessments = await _context.Assesment
                                                .ToArrayAsync();

                // Return an empty array if assessments are null
                return assessments ?? new Assesment[0];
            }
            catch (Exception ex)
            {
                // Log the error
                _logger.LogError(ex, "Error retrieving assessments.");
                throw;
            }
        }


        public async Task<Assesment> GetAssesmentAsync(Guid assesmentId)
        {
            IQueryable<Assesment> query = _context.Assesment.Where(c => c.AssesmentId == assesmentId);
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
