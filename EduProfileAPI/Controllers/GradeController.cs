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
       

        public GradeController(IGradeRepository gradeRepo)
        {
            _gradeRepo = gradeRepo;
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

        // Create Grade

        [HttpPost]
        [Route("CreateGrade")]
        public async Task<IActionResult> CreateGradeAsync([FromBody] CreateGradeViewModel model)
        {
            try
            {
                if (model == null)
                {
                    return BadRequest("Invalid grade data.");
                }

                // Find the StudentEducationPhase
                var createdGrade = await _gradeRepo.CreateGradeAsync(model);
                if (createdGrade == null)
                {
                    return NotFound("Education phase not found.");
                }
                // return CreatedAtAction(nameof(GetAllGradesAsync), new { id = createdGrade.GradeId }, createdGrade);
                return Ok(createdGrade);

            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Database Failure: {ex.Message}");
            }
        }

        // Update Grade
        [HttpPut]
        [Route("UpdateGrade")]
        public async Task<IActionResult> UpdateGradeAsync([FromBody] UpdateGradeViewModel model)
        {
            try
            {
                if (model == null)
                {
                    return BadRequest("Invalid grade data.");
                }

                var updatedGrade = await _gradeRepo.UpdateGradeAsync(model);
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


        // Delete Grade

        [HttpDelete]
        [Route("DeleteGrade/{id}")]
        public async Task<IActionResult> DeleteGradeAsync(Guid id)
        {
            try
            {
                var result = await _gradeRepo.DeleteGradeAsync(id);
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

        
    
