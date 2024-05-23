using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using EduProfileAPI.Repositories.Interfaces;
using EduProfileAPI.ViewModels;

namespace EduProfileAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectController : ControllerBase
    {
        private readonly ISubjectRepository _subjectRepository;
        public SubjectController(ISubjectRepository subjectRepository)
        {
            _subjectRepository = subjectRepository;
        }
        //Get all subjects
        [HttpGet]
        [Route("GetAllSubject")]
        public async Task<IActionResult> GetAllSubjectsAsync()
        {
            try
            {
                var results = await _subjectRepository.GetAllSubjectAsync();
                return Ok(results);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Database Failure: {ex.Message}");
            }
        }
        //Get subject by id come back to this
        [HttpGet]
        [Route("GetSubject/{subjectId}")]
        public async Task<IActionResult> GetSubjectByIdAsync(Guid subjectId)
        {
            try
            {
                var results = await _subjectRepository.GetSubjectByIdAsync(subjectId);
                if (results == null)
                {
                    return NotFound("Subject does not exist");
                }
                return Ok(results);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Database Failure: {ex.Message}");
            }
        }

        //Create new subject
        [HttpPost]
        [Route("CreateSubject")]
        public async Task<IActionResult> CreateSubjectAsync([FromBody] SubjectViewModel subjectModel)
        {
            try
            {
                if (subjectModel == null)
                { 
                    return BadRequest("Invalid subject data.");
                }

                //add the finding of employee and class later
                var createdSubject = await _subjectRepository.CreateSubjectAsync(subjectModel);
                
                return Ok(subjectModel);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Database Failure: {ex.Message}");
            }
        
        }

        //Update subject
        [HttpPut]
        [Route("UpdateSubject")]
        public async Task<IActionResult> UpdateSubjectAsync([FromBody] SubjectViewModel subjectModel)
        {
            try
            {
                if (subjectModel == null)
                {
                    return BadRequest("Invalid subject data.");
                }

                var updatedSubject = await _subjectRepository.UpdateSubjectAsync(subjectModel);
                if (updatedSubject == null)
                {
                    return NotFound("Subject not found.");
                }
                return Ok(updatedSubject);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Database Failure: {ex.Message}");
            }
        }

        //delete subject
        [HttpDelete]
        [Route("DeleteSubject/{subjectId}")]
        public async Task<IActionResult> DeleteSubjectAsync(Guid subjectId)
        {
            try
            {
                var deletedSubject = await _subjectRepository.DeleteSubjectAsync(subjectId);
                if (!deletedSubject)
                {
                    return NotFound("Subject not found.");
                }
                return Ok(deletedSubject);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Database Failure: {ex.Message}");
            }
        }
    }
}
