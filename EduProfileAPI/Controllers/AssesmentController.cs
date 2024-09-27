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
        private readonly IAssesment _assesmentRepository;
        private readonly ILogger<AssesmentController> _logger;
        public AssesmentController(IAssesment assesmentRepository, ILogger<AssesmentController> logger)
        {
            _assesmentRepository = assesmentRepository;
            _logger = logger;
        }

        [HttpGet]
        [Route("GetAllAssesments")]
        public async Task<IActionResult> GetAllAssesments()
        {
            try
            {
                var assessments = await _assesmentRepository.GetAllAssesmentsAsync();

                if (assessments == null || !assessments.Any())
                {
                    return NotFound("No assessments found.");
                }

                return Ok(assessments);
            }
            catch (Exception ex)
            {
                // Log the error and return a more user-friendly error message
                _logger.LogError(ex, "An error occurred while fetching assessments.");
                return StatusCode(500, "Internal Server Error: Data is null or unavailable.");
            }
        }

        [HttpGet]
        [Route("GetAssesment/{assesmentId}")] 
        public async Task<IActionResult> GetAssesment(Guid assesmentId)
        {
            try
            {
                var results = await _assesmentRepository.GetAssesmentAsync(assesmentId);

                if (results == null) return NotFound("Assesment does not exist");

                return Ok(results);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }

        [HttpPost]
        [Route("AddAssesment")]
        public async Task<IActionResult> AddAssesment([FromBody] AssesmentVM cvm, [FromQuery] Guid userId)
        {
            var assesment = new Assesment
            {
                SubjectId = cvm.SubjectId,
                EmployeeId = cvm.EmployeeId,
                AssesmentName = cvm.AssesmentName,
                AchievableMark = cvm.AchievableMark,
                AssesmentDate = cvm.AssesmentDate,
                AssesmentType = cvm.AssesmentType,
                AssesmentWeighting = cvm.AssesmentWeighting,
                Term = cvm.Term
            };

            try
            {
                await _assesmentRepository.AddAssessmentAsync(assesment, userId); // Log the create action
                await _assesmentRepository.SaveChangesAsync();
                return Ok(assesment);
            }
            catch (Exception ex)
            {
                return BadRequest($"Internal Server Error: {ex.Message}");
            }
        }

        [HttpPut]
        [Route("EditAssesment/{assesmentId}")]
        public async Task<ActionResult<AssesmentVM>> EditAssesment(Guid assesmentId, [FromBody] AssesmentVM model, [FromQuery] Guid userId)
        {
            try
            {
                var existingAssesment = await _assesmentRepository.GetAssesmentAsync(assesmentId);
                if (existingAssesment == null) return NotFound("The assesment does not exist");

                var updatedAssesment = new Assesment
                {
                    AssesmentId = assesmentId,
                    EmployeeId = model.EmployeeId,
                    SubjectId = model.SubjectId,
                    AssesmentName = model.AssesmentName,
                    AssesmentDate = model.AssesmentDate,
                    AssesmentType = model.AssesmentType,
                    AssesmentWeighting = model.AssesmentWeighting,
                    AchievableMark = model.AchievableMark,
                    Term = model.Term
                };

                await _assesmentRepository.UpdateAssessmentAsync(updatedAssesment, existingAssesment, userId); // Log the update action
                await _assesmentRepository.SaveChangesAsync();

                return Ok(updatedAssesment);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }


        [HttpDelete]
        [Route("DeleteAssesment/{assesmentId}")]
        public async Task<IActionResult> DeleteAssesment(Guid assesmentId, [FromQuery] Guid userId)
        {
            try
            {
                var existingAssesment = await _assesmentRepository.GetAssesmentAsync(assesmentId);
                if (existingAssesment == null) return NotFound("The assesment does not exist");

                await _assesmentRepository.DeleteAssessmentAsync(existingAssesment, userId); // Log the delete action
                await _assesmentRepository.SaveChangesAsync();

                return Ok(existingAssesment);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }


        [HttpGet]
        [Route("GetAssesmentsByTerm/{term}")]
        public async Task<IActionResult> GetAssesmentsByTerm(int term)
        {
            try
            {
                var results = await _assesmentRepository.GetAssessmentsByTermAsync(term);

                if (results == null || !results.Any())
                    return NotFound($"No assessments found for term {term}");

                return Ok(results);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }


    }
}
