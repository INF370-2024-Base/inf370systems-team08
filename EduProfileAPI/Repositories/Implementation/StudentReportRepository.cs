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

            var parentEmail = await _context.Parent.FirstOrDefaultAsync(pe => pe.ParentId == student.ParentId);

            var assessmentMarks = await _context.AssesmentMark
                .Where(am => am.StudentId == studentId)
                .ToListAsync();

            var assessments = await _context.Assesment
                .Where(a => assessmentMarks.Select(am => am.AssesmentId).Contains(a.AssesmentId))
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
                Assessments = assessments.Select(a => new StudentProgressReportViewModel.AssessmentReport
                {
                    Assesment = a,
                    MarkAchieved = assessmentMarks.FirstOrDefault(am => am.AssesmentId == a.AssesmentId)?.MarkAchieved ?? 0
                }).ToList(),
                Merits = merits,
                Incidents = incidents,
                ParentEmail = parentEmail.Parent1Email
            };

            return report;
        }
    }
}
