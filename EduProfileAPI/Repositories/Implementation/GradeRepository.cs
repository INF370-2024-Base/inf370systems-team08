using EduProfileAPI.DataAccessLayer;
using EduProfileAPI.Models;
using EduProfileAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using EduProfileAPI.ViewModels;
using Newtonsoft.Json;

namespace EduProfileAPI.Repositories.Implementation
{
    public class GradeRepository : IGradeRepository //what is whrong here? IGradeRepository is not implemented in GradeRepository class
    {
        private readonly EduProfileDbContext _context;
        private readonly IAuditTrail _auditTrailRepo;

        public GradeRepository(EduProfileDbContext context, IAuditTrail auditTrailRepo)
        {
            _context = context;
            _auditTrailRepo = auditTrailRepo;
        }


        // Call the stored procedure to retrieve all grades
        public async Task<List<Grade>> GetAllGradesAsync()
        {
            return await _context.Grade
                .FromSqlRaw("EXEC GetAllGrades")
                .ToListAsync();
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


        public async Task<GradeViewModel> CreateGradeAsync(CreateGradeViewModel model, Guid userId)
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

            // Log creation in audit trail
            var auditTrail = new AuditTrail
            {
                UserId = userId,
                Action = "CREATE",
                EntityName = "Grade",
                AffectedEntityID = grade.GradeId,
                NewValue = JsonConvert.SerializeObject(grade),
                TimeStamp = DateTime.UtcNow
            };
            _context.AuditTrail.Add(auditTrail);
            await _context.SaveChangesAsync();

            return new GradeViewModel
            {
                GradeId = grade.GradeId,
                EducationPhaseName = educationPhase.EducationPhaseName,
                GradeLevel = grade.GradeLevel,
            };
        }

        public async Task<GradeViewModel> UpdateGradeAsync(UpdateGradeViewModel model, Guid userId)
        {
            var grade = await _context.Grade.FirstOrDefaultAsync(g => g.GradeId == model.GradeId);
            if (grade == null)
            {
                return null;
            }

            var oldGrade = JsonConvert.SerializeObject(grade);  // Capture old state

            var educationPhase = await _context.StudentEducationPhase.FirstOrDefaultAsync(e => e.StudentEducationPhaseId == model.StudentEducationPhaseId);
            if (educationPhase == null)
            {
                return null;
            }

            grade.StudentEducationPhaseId = model.StudentEducationPhaseId;
            grade.GradeLevel = model.GradeLevel;

            _context.Grade.Update(grade);
            await _context.SaveChangesAsync();

            // Log update in audit trail
            var auditTrail = new AuditTrail
            {
                UserId = userId,
                Action = "UPDATE",
                EntityName = "Grade",
                AffectedEntityID = grade.GradeId,
                OldValue = oldGrade,
                NewValue = JsonConvert.SerializeObject(grade),
                TimeStamp = DateTime.UtcNow
            };
            _context.AuditTrail.Add(auditTrail);
            await _context.SaveChangesAsync();

            return new GradeViewModel
            {
                GradeId = grade.GradeId,
                GradeLevel = grade.GradeLevel,
                EducationPhaseName = educationPhase.EducationPhaseName
            };
        }

        public async Task<bool> DeleteGradeAsync(Guid id, Guid userId)
        {
            var grade = await _context.Grade.FirstOrDefaultAsync(g => g.GradeId == id);
            if (grade == null)
            {
                return false;
            }

            var oldGrade = JsonConvert.SerializeObject(grade);  // Capture old state before deletion

            _context.Grade.Remove(grade);
            await _context.SaveChangesAsync();

            // Log delete action in audit trail
            var auditTrail = new AuditTrail
            {
                UserId = userId,
                Action = "DELETE",
                EntityName = "Grade",
                AffectedEntityID = grade.GradeId,
                OldValue = oldGrade,
                TimeStamp = DateTime.UtcNow
            };
            _context.AuditTrail.Add(auditTrail);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}