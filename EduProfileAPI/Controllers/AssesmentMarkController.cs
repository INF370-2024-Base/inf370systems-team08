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

        public AssesmentMarkController(IAssesmentMark assesmentMarkRepository)
        {
            _assesmentMarkRepository = assesmentMarkRepository;
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
        public async Task<IActionResult> AddAssesmentMark(AssesmentMarkVM cvm)
        {
            var assesmentMark = new AssesmentMark { StudentId = cvm.StudentId, AssesmentId = cvm.AssesmentId, MarkAchieved = cvm.MarkAchieved };

            try
            {
                _assesmentMarkRepository.Add(assesmentMark);
                await _assesmentMarkRepository.SaveChangesAsync();
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

            return Ok(assesmentMark);
        }

        [HttpPut]
        [Route("EditAssesmentMark/{studentId}/{assesmentId}")]
        public async Task<ActionResult<AssesmentMarkVM>> EditAssesmentMark(Guid studentId, Guid assesmentId, [FromBody] AssesmentMarkVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var existingAssesmentMark = await _assesmentMarkRepository.GetAssesmentMarkAsync(studentId, assesmentId);
                if (existingAssesmentMark == null)
                {
                    return NotFound($"The assesment mark does not exist");
                }

                existingAssesmentMark.StudentId = model.StudentId;
                existingAssesmentMark.AssesmentId = model.AssesmentId;
                existingAssesmentMark.MarkAchieved = model.MarkAchieved;

                if (await _assesmentMarkRepository.SaveChangesAsync())
                {
                    return Ok(existingAssesmentMark);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }

            return BadRequest("Your request is invalid.");
        }



    }
}
