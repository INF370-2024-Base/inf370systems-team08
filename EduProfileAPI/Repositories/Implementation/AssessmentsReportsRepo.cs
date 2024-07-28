using EduProfileAPI.DataAccessLayer;
using EduProfileAPI.Repositories.Interfaces;
using EduProfileAPI.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace EduProfileAPI.Repositories.Implementation
{
    public class AssessmentsReportsRepo : IAssessmentsReportsRepo
    {
        private readonly EduProfileDbContext _context;

        public AssessmentsReportsRepo(EduProfileDbContext context)
        {
            _context = context;
        }

        public async Task<List<AssessmentAverageReportViewModel>> GetAssessmentAverageReport()
        {
            var result = await _context.Assesment
                .GroupJoin(
                    _context.AssesmentMark,
                    assessment => assessment.AssesmentId,
                    mark => mark.AssesmentId,
                    (assessment, marks) => new
                    {
                        Assessment = assessment,
                        Marks = marks
                    }
                )
                .Select(x => new AssessmentAverageReportViewModel
                {
                    AssesmentId = x.Assessment.AssesmentId,
                    AssesmentName = x.Assessment.AssesmentName,
                    AssesmentDate = x.Assessment.AssesmentDate,
                    AverageMark = x.Marks.Any() ? x.Marks.Average(m => m.MarkAchieved) : 0
                })
                .ToListAsync();

            return result;
        }

        public async Task<List<AssessmentHighestMarkReportViewModel>> GetAssessmentHighestMarkReport()
        {
            var result = await _context.Assesment
                .Join(
                    _context.AssesmentMark,
                    assessment => assessment.AssesmentId,
                    mark => mark.AssesmentId,
                    (assessment, mark) => new
                    {
                        Assessment = assessment,
                        Mark = mark
                    }
                )
                .Join(
                    _context.Student,
                    am => am.Mark.StudentId,
                    student => student.StudentId,
                    (am, student) => new
                    {
                        am.Assessment,
                        am.Mark,
                        Student = student
                    }
                )
                .GroupBy(x => new { x.Assessment.SubjectId, x.Assessment.AssesmentId, x.Assessment.AssesmentName })
                .Select(g => new AssessmentHighestMarkReportViewModel
                {
                    SubjectId = g.Key.SubjectId,
                    AssesmentId = g.Key.AssesmentId,
                    AssesmentName = g.Key.AssesmentName,
                    HighestMark = g.Max(x => x.Mark.MarkAchieved),
                    StudentFirstName = g.Where(x => x.Mark.MarkAchieved == g.Max(m => m.Mark.MarkAchieved)).Select(s => s.Student.FirstName).FirstOrDefault(),
                    StudentLastName = g.Where(x => x.Mark.MarkAchieved == g.Max(m => m.Mark.MarkAchieved)).Select(s => s.Student.LastName).FirstOrDefault()
                })
                .ToListAsync();

            return result;
        }
    }
}
