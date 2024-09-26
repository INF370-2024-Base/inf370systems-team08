using EduProfileAPI.Models;
using EduProfileAPI.Repositories.Interfaces;
using EduProfileAPI.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
namespace EduProfileAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssesmentController : ControllerBase
    {
        private readonly IAssesment _assessmentRepository;

        public AssesmentController(IAssesment assessmentRepository)
        {
            _assessmentRepository = assessmentRepository;
        }

        // GET: api/Assessment
        [HttpGet]
        [Route("GetAllAssesments")]
        public async Task<IActionResult> GetAllAssessments()
        {
            var assessments = await _assessmentRepository.GetAllAssessmentsAsync();
            return Ok(assessments);
        }

        // GET: api/Assessment/{id}
        [HttpGet]
        [Route("GetAssesment/{assesmentId}")]
        public async Task<IActionResult> GetAssessmentById(Guid id)
        {
            var assessment = await _assessmentRepository.GetAssessmentByIdAsync(id);
            if (assessment == null)
            {
                return NotFound();
            }
            return Ok(assessment);
        }

        // POST: api/Assessment
        [HttpPost]
        [Route("AddAssessment")]
        public async Task<IActionResult> AddAssessment([FromBody] Assesment assessment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _assessmentRepository.AddAssessmentAsync(assessment);
            return CreatedAtAction(nameof(GetAssessmentById), new { id = assessment.AssesmentId }, assessment);
        }

        // PUT: api/Assessment/{id}
        [HttpPut]
        [Route("UpdateAssessment/{id}")]
        public async Task<IActionResult> UpdateAssessment(Guid id, [FromBody] Assesment assessment)
        {
            if (id != assessment.AssesmentId)
            {
                return BadRequest("Assessment ID mismatch");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _assessmentRepository.UpdateAssessmentAsync(assessment);
            return NoContent();
        }

        // DELETE: api/Assessment/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAssessment(Guid id)
        {
            await _assessmentRepository.DeleteAssessmentAsync(id);
            return NoContent();
        }


    }
}
