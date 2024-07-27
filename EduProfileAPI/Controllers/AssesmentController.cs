using EduProfileAPI.Models;
using EduProfileAPI.Repositories.Interfaces;
using EduProfileAPI.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EduProfileAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssesmentController : ControllerBase
    {
        private readonly IAssesment _assesmentRepository;

        public AssesmentController(IAssesment assesmentRepository)
        {
            _assesmentRepository = assesmentRepository;
        }

        [HttpGet]
        [Route("GetAllAssesments")] 
        public async Task<IActionResult> GetAllAssesments()
        {
            try
            {
                var results = await _assesmentRepository.GetAllAssesmentsAsync();
                return Ok(results);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
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
        public async Task<IActionResult> AddAssesment(AssesmentVM cvm)
        {
            var assesment = new Assesment { SubjectId = cvm.SubjectId, EmployeeId = cvm.EmployeeId, AssesmentName = cvm.AssesmentName, AchievableMark = cvm.AchievableMark, AssesmentDate = cvm.AssesmentDate, AssesmentType = cvm.AssesmentType, AssesmentWeighting = cvm.AssesmentWeighting };

            try
            {
                _assesmentRepository.Add(assesment);
                await _assesmentRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest($"Internal Server Error: {ex.Message}");
            }

            return Ok(assesment);
        }

        [HttpPut]
        [Route("EditAssesment/{assesmentId}")]
        public async Task<ActionResult<AssesmentVM>> EditAssesment(Guid assesmentId, AssesmentVM model)
        {
            try
            {
                var existingAssesment = await _assesmentRepository.GetAssesmentAsync(assesmentId);
                if (existingAssesment == null) return NotFound($"The assesment does not exist");
                existingAssesment.EmployeeId = model.EmployeeId;
                existingAssesment.SubjectId = model.SubjectId;
                existingAssesment.AssesmentName = model.AssesmentName;
                existingAssesment.AssesmentDate = model.AssesmentDate;
                existingAssesment.AssesmentType = model.AssesmentType;
                existingAssesment.AssesmentWeighting = model.AssesmentWeighting;
                existingAssesment.AchievableMark = model.AchievableMark;

                if (await _assesmentRepository.SaveChangesAsync())
                {
                    return Ok(existingAssesment);
                }

            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");

            }
            return BadRequest("Your request is invalid.");


        }

        [HttpDelete]
        [Route("DeleteAssesment/{assesmentId}")]
        public async Task<IActionResult> DeleteAssesment(Guid assesmentId)
        {
            try
            {
                var existingAssesment = await _assesmentRepository.GetAssesmentAsync(assesmentId);
                if (existingAssesment == null) return NotFound($"The assesment does not exist");
                _assesmentRepository.Delete(existingAssesment);

                if (await _assesmentRepository.SaveChangesAsync()) return Ok(existingAssesment);
            }
            catch (Exception)
            {

                return StatusCode(500, "Internal Server Error. Please contact support.");
            }

            return BadRequest("Your request is invalid");
        }
    }
}
