using EduProfileAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EduProfileAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssessmentsGradeController : ControllerBase
    {
        private readonly IAssessmentRepository _assessmentRepository;

        public AssessmentsGradeController(IAssessmentRepository assessmentRepository)
        {
            _assessmentRepository = assessmentRepository;
        }

        [HttpGet("student/{studentId}")]
        public async Task<IActionResult> GetStudentAssessments(Guid studentId)
        {
            var result = await _assessmentRepository.GetStudentAssessments(studentId);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
    }
}
