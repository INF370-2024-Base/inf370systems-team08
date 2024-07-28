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
    }
}
