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
                TermId = cvm.TermId
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
        
      
        // PUT: api/Assessment/{id}
        [HttpPut]
        [Route("EditAssesment/{assesmentId}")]
        public async Task<ActionResult<AssesmentVM>> EditAssesment(Guid assesmentId, [FromBody] AssesmentVM model, [FromQuery] Guid userId)
        {
            try
            {
                var existingAssesment = await _assesmentRepository.GetAssessmentByIdAsync(assesmentId);
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
                    TermId = model.TermId
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
                var existingAssesment = await _assesmentRepository.GetAssessmentByIdAsync(assesmentId);
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


    }
}
