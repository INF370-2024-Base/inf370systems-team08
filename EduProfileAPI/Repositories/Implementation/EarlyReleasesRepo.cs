using EduProfileAPI.DataAccessLayer;
using EduProfileAPI.Models;
using EduProfileAPI.Repositories.Interfaces;
using EduProfileAPI.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace EduProfileAPI.Repositories.Implementation
{
    public class EarlyReleasesRepo : IEarlyReleasesRepo
    {
        private readonly EduProfileDbContext _context;

        public EarlyReleasesRepo( EduProfileDbContext context)
        {
            _context = context;
        }

        public async Task<EarlyReleases> CreateEarlyRelease(CreateEarlyReleaseVM model)
        {
            var student = await _context.Student.FirstOrDefaultAsync(s => s.StudentId == model.StudentId);

            if (student == null)
                return null; 

            var parent = await _context.Parent.FirstOrDefaultAsync(p => p.ParentId == student.ParentId);

            if (parent == null)
                return null; 

            var earlyRelease = new EarlyReleases
            {
                EarlyRelId = Guid.NewGuid(),
                StudentId = model.StudentId,
                ParentId = parent.ParentId,
                EmployeeId = model.EmployeeId,
                DateOfEarlyRelease = model.DateOfEarlyRelease,
                ReasonForEarlyRelease = model.ReasonForEarlyRelease,
                SignerRelationship = model.SignerRelationship,
                SignerName = model.SignerName,
                SignerIDNumber = model.SignerIDNumber
            };

            _context.EarlyReleases.Add(earlyRelease);
            await _context.SaveChangesAsync();

            return earlyRelease;

        }
    }
}
