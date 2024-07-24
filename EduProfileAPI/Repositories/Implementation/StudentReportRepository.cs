using EduProfileAPI.DataAccessLayer;
using EduProfileAPI.ViewModels;
using Microsoft.EntityFrameworkCore;
using EduProfileAPI.Repositories.Interfaces;

namespace EduProfileAPI.Repositories.Implementation
{
    public class StudentReportRepository : IStudentReportRepository
    {
        private readonly EduProfileDbContext _context;
        public StudentReportRepository(EduProfileDbContext context)
        {
            _context = context;
        }
        public async Task<StudentProgressReportViewModel> GetStudentProgressReportAsync(Guid studentId)
        {
            var student = await _context.Student
             .FirstOrDefaultAsync(s => s.StudentId == studentId);

            if (student == null)
            {
                return null;
            }

            var assessments = await _context.AssesmentMark
                .Where(am => am.StudentId == studentId)
                .Select(am => new
                {
                    am.MarkAchieved,
                    Assessment = _context.Assesment.FirstOrDefault(a => a.AssesmentId == am.AssesmentId)
                })
                .ToListAsync();

            var merits = await _context.Merit
                .Where(m => m.StudentId == studentId)
                .ToListAsync();

            var incidents = await _context.StudentIncident
                .Where(i => i.StudentId == studentId)
                .ToListAsync();

            var report = new StudentProgressReportViewModel
            {
                StudentName = $"{student.FirstName} {student.LastName}",
                Assessments = assessments.Select(a => a.Assessment).ToList(),
                Merits = merits,
                Incidents = incidents
            };

            // Populate the assessment grades from AssessmentMarks
            foreach (var assessment in report.Assessments)
            {
                var assessmentMark = assessments.FirstOrDefault(a => a.Assessment.AssesmentId == assessment.AssesmentId);
                if (assessmentMark != null)
                {
                    assessment.AssesmentGrades = ParseMarkAchieved(assessmentMark.MarkAchieved) ?? 0;
                }
            }

            return report;
        }

        private int? ParseMarkAchieved(string markAchieved)
        {
            if (int.TryParse(markAchieved, out int mark))
            {
                return mark;
            }
            return null;
        }
    }
}
