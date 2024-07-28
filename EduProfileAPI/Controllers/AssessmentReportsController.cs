using EduProfileAPI.Repositories.Interfaces;
using EduProfileAPI.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EduProfileAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssessmentReportsController : ControllerBase
    {
        private readonly IAssessmentsReportsRepo _assessmentsReportsRepo;

        public AssessmentReportsController(IAssessmentsReportsRepo assessmentsReportsRepo)
        {
            _assessmentsReportsRepo = assessmentsReportsRepo;
        }

        [HttpGet("average-report")]
        public async Task<ActionResult<List<AssessmentAverageReportViewModel>>> GetAssessmentAverageReport()
        {
            var report = await _assessmentsReportsRepo.GetAssessmentAverageReport();
            return Ok(report);
        }
    }
}
