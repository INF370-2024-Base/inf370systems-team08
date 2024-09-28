using EduProfileAPI.Repositories.Interfaces;
using EduProfileAPI.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using EduProfileAPI.Models;
using EduProfileAPI.DataAccessLayer;
using Microsoft.AspNetCore.Http.HttpResults;



namespace EduProfileAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GradeController : ControllerBase
    {
        private readonly IGradeRepository _gradeRepo;
        private readonly IAuditTrail _auditTrailRepo; // Add AuditTrailRepository


        public GradeController(IGradeRepository gradeRepo, IAuditTrail auditTrailRepo)
        {
            _gradeRepo = gradeRepo;
            _auditTrailRepo = auditTrailRepo;
        }

        [HttpGet]
        [Route("GetAllGrades")]
        public async Task<IActionResult> GetAllGradesAsync()
        {
            try
            {
                var results = await _gradeRepo.GetAllGradesAsync();
                return Ok(results);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Database Failure: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("CreateGrade")]
        public async Task<IActionResult> CreateGradeAsync([FromBody] CreateGradeViewModel model, [FromQuery] Guid userId)
        {
            if (model == null || userId == Guid.Empty)
            {
                return BadRequest("Invalid grade data or missing user ID.");
            }

            try
            {
                var createdGrade = await _gradeRepo.CreateGradeAsync(model, userId);
                if (createdGrade == null)
                {
                    return NotFound("Education phase not found.");
                }

                return Ok(createdGrade);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Database Failure: {ex.Message}");
            }
        }

        [HttpPut]
        [Route("UpdateGrade")]
        public async Task<IActionResult> UpdateGradeAsync([FromBody] UpdateGradeViewModel model, [FromQuery] Guid userId)
        {
            if (model == null || userId == Guid.Empty)
            {
                return BadRequest("Invalid grade data or missing user ID.");
            }

            try
            {
                var updatedGrade = await _gradeRepo.UpdateGradeAsync(model, userId);
                if (updatedGrade == null)
                {
                    return NotFound("Grade or education phase not found.");
                }

                return Ok(updatedGrade);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Database Failure: {ex.Message}");
            }
        }

        [HttpDelete]
        [Route("DeleteGrade/{id}")]
        public async Task<IActionResult> DeleteGradeAsync(Guid id, [FromQuery] Guid userId)
        {
            if (userId == Guid.Empty)
            {
                return BadRequest("Missing user ID.");
            }

            try
            {
                var result = await _gradeRepo.DeleteGradeAsync(id, userId);
                if (!result)
                {
                    return NotFound("Grade not found.");
                }

                return Ok("Grade deleted successfully.");
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Database Failure: {ex.Message}");
            }
        }

        // Get Grade by ID
        [HttpGet]
        [Route("GetGrade/{id}")]
        public async Task<IActionResult> GetGradeByIdAsync(Guid id)
        {
            try
            {
                var result = await _gradeRepo.GetGradeByIdAsync(id);
                if (result == null)
                {
                    return NotFound("Grade not found.");
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Database Failure: {ex.Message}");
            }
        }
    }
}

        
    
