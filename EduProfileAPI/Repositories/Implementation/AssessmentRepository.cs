using EduProfileAPI.DataAccessLayer;
using EduProfileAPI.Repositories.Interfaces;
using EduProfileAPI.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace EduProfileAPI.Repositories.Implementation
{
    public class AssessmentRepository : IAssessmentRepository
    {
        private readonly EduProfileDbContext _context;

        public AssessmentRepository(EduProfileDbContext context)
        {
            _context = context;
        }

        public async Task<StudentAssessmentViewModel> GetStudentAssessments(Guid studentId)
        {
            var student = await _context.Student.FindAsync(studentId);
            if (student == null)
            {
                return null;
            }

            var assessments = await _context.AssesmentMark
                .Where(am => am.StudentId == studentId)
                .Select(am => new
                {
                    MarkAchieved = ParseMarkAchieved(am.MarkAchieved),
                    Assessment = _context.Assesment.FirstOrDefault(a => a.AssesmentId == am.AssesmentId)
                })
                .ToListAsync();

            return new StudentAssessmentViewModel
            {
                StudentId = studentId,
                StudentName = $"{student.FirstName} {student.LastName}",
                AssessmentMarks = assessments.Select(a => new AssessmentMarkViewModel
                {
                    AssessmentId = a.Assessment.AssesmentId,
                    AssessmentName = a.Assessment.AssesmentName,
                    AssessmentDate = a.Assessment.AssesmentDate,
                    AssessmentType = a.Assessment.AssesmentType,
                    MarkAchieved = a.MarkAchieved ?? 0, // Default to 0 if parsing fails
                    AchievableMark = a.Assessment.AchievableMark
                }).ToList()
            };
        }

        private static int? ParseMarkAchieved(string markAchieved)
        {
            if (int.TryParse(markAchieved, out int mark))
            {
                return mark;
            }
            return null;
        }
    }
}
