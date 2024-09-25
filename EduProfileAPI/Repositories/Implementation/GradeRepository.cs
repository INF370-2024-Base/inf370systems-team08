using EduProfileAPI.DataAccessLayer;
using EduProfileAPI.Models;
using EduProfileAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using EduProfileAPI.ViewModels;

namespace EduProfileAPI.Repositories.Implementation
{
    public class GradeRepository : IGradeRepository //what is whrong here? IGradeRepository is not implemented in GradeRepository class
    {
        private readonly EduProfileDbContext _context;

        public GradeRepository(EduProfileDbContext context)
        {
            _context = context;

        }

        //public async Task<Grade[]> GetAllGradesAsync()
        //{
        //    IQueryable<Grade> query = _context.Grade;
        //    return await query.ToArrayAsync();
        //}

        // Call the stored procedure to retrieve all grades
        public async Task<List<Grade>> GetAllGradesAsync()
        {
            return await _context.Grade
                .FromSqlRaw("EXEC GetAllGrades")
                .ToListAsync();
        }

        public async Task<GradeViewModel> CreateGradeAsync(CreateGradeViewModel model)
        {
            var educationPhase = await _context.StudentEducationPhase.FirstOrDefaultAsync(e => e.StudentEducationPhaseId == model.StudentEducationPhaseId);
            if (educationPhase == null)
            {
                return null;
            }

            var grade = new Grade
            {
                GradeId = Guid.NewGuid(),
                StudentEducationPhaseId = model.StudentEducationPhaseId,
                GradeLevel = model.GradeLevel,
            };

            await _context.Grade.AddAsync(grade);
            await _context.SaveChangesAsync();


            return new GradeViewModel
            {
                GradeId = grade.GradeId,
                EducationPhaseName = educationPhase.EducationPhaseName,
                GradeLevel = grade.GradeLevel,
            };
        }

        //GET GRADE BY ID Business Logic
        public async Task<GradeViewModel> GetGradeByIdAsync(Guid id)
        {
            var grade = await _context.Grade.Include(g => g.StudentEducationPhase).FirstOrDefaultAsync(g => g.GradeId == id);
            if (grade == null)
            {
                return null;
            }

            return new GradeViewModel
            {
                GradeId = grade.GradeId,
                GradeLevel = grade.GradeLevel,
                EducationPhaseName = grade.StudentEducationPhase.EducationPhaseName
            };
        }


        //UPDATE grade Business Logic
        public async Task<GradeViewModel> UpdateGradeAsync(UpdateGradeViewModel model)
        {
            var grade = await _context.Grade.FirstOrDefaultAsync(g => g.GradeId == model.GradeId);
            if (grade == null)
            {
                return null;
            }

            var educationPhase = await _context.StudentEducationPhase.FirstOrDefaultAsync(e => e.StudentEducationPhaseId == model.StudentEducationPhaseId);
            if (educationPhase == null)
            {
                return null;
            }

            grade.StudentEducationPhaseId = model.StudentEducationPhaseId;
            grade.GradeLevel = model.GradeLevel;

            _context.Grade.Update(grade);
            await _context.SaveChangesAsync();

            return new GradeViewModel
            {
                GradeId = grade.GradeId,
                GradeLevel = grade.GradeLevel,
                EducationPhaseName = educationPhase.EducationPhaseName
            };
        }

        //delete grade Business Logic 
        public async Task<bool> DeleteGradeAsync(Guid id)
        {
            var grade = await _context.Grade.FirstOrDefaultAsync(g => g.GradeId == id);
            if (grade == null)
            {
                return false;
            }

            _context.Grade.Remove(grade);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}