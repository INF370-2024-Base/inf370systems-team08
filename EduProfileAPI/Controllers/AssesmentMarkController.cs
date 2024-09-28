using EduProfileAPI.Models;
using EduProfileAPI.Repositories.Interfaces;
using EduProfileAPI.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduProfileAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssesmentMarkController : ControllerBase
    {
        private readonly IAssesmentMark _assesmentMarkRepository;
        private readonly IAuditTrail _auditTrailRepository;


        public AssesmentMarkController(IAssesmentMark assesmentMarkRepository, IAuditTrail auditTrailRepository)
        {
            _assesmentMarkRepository = assesmentMarkRepository;
            _auditTrailRepository = auditTrailRepository;
        }

        [HttpGet]
        [Route("GetAllAssesmentMarks")]
        public async Task<IActionResult> GetAllAssesmentMarks()
        {
            try
            {
                var results = await _assesmentMarkRepository.GetAllAssesmentMarksAsync();
                return Ok(results);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("GetAssesmentMark/{studentId}/{assesmentId}")]
        public async Task<IActionResult> GetAssesmentMark(Guid studentId, Guid assesmentId)
        {
            try
            {
                var results = await _assesmentMarkRepository.GetAssesmentMarkAsync(studentId, assesmentId);

                if (results == null) return NotFound("Assesment Mark does not exist");

                return Ok(results);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }

        [HttpPost]
        [Route("AddAssesmentMark")]
        public async Task<IActionResult> AddAssesmentMark([FromBody] AssesmentMarkVM cvm, [FromQuery] Guid userId)
        {
            if (userId == Guid.Empty)
            {
                return BadRequest("User ID is required.");
            }

            var assesmentMark = new AssesmentMark
            {
                StudentId = cvm.StudentId,
                AssesmentId = cvm.AssesmentId,
                MarkAchieved = cvm.MarkAchieved
            };

            try
            {
                await _assesmentMarkRepository.AddAssesmentMarkAsync(assesmentMark, userId); // Log create action
                await _assesmentMarkRepository.SaveChangesAsync();

                return Ok(assesmentMark);
            }
            catch (DbUpdateException dbEx)
            {
                var innerException = dbEx.InnerException?.Message ?? dbEx.Message;
                return StatusCode(500, $"Internal Server Error: {innerException}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        // Edit an assessment mark with audit trail
        [HttpPut]
        [Route("EditAssesmentMark/{studentId}/{assesmentId}")]
        public async Task<ActionResult<AssesmentMarkVM>> EditAssesmentMark(Guid studentId, Guid assesmentId, [FromBody] AssesmentMarkVM model, [FromQuery] Guid userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (userId == Guid.Empty)
            {
                return BadRequest("User ID is required.");
            }

            try
            {
                var existingAssesmentMark = await _assesmentMarkRepository.GetAssesmentMarkAsync(studentId, assesmentId);
                if (existingAssesmentMark == null)
                {
                    return NotFound("The assesment mark does not exist");
                }

                var updatedAssesmentMark = new AssesmentMark
                {
                    StudentId = model.StudentId,
                    AssesmentId = model.AssesmentId,
                    MarkAchieved = model.MarkAchieved
                };

                await _assesmentMarkRepository.UpdateAssesmentMarkAsync(updatedAssesmentMark, existingAssesmentMark, userId); // Log update action
                await _assesmentMarkRepository.SaveChangesAsync();

                return Ok(updatedAssesmentMark);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }



    }
}
