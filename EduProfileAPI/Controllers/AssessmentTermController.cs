using EduProfileAPI.Models;
using EduProfileAPI.Repositories.Interfaces;
using EduProfileAPI.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace EduProfileAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AssessmentTermController : ControllerBase
    {
        private readonly IAssesmentTerm _termRepository;
        private readonly ILogger<AssessmentTermController> _logger;
        public AssessmentTermController(IAssesmentTerm termRepository, ILogger<AssessmentTermController> logger)
        {
            _termRepository = termRepository;
            _logger = logger;
        }

        [HttpPost]
        [Route("AddAssesmentTerm")]
        public async Task<IActionResult> AddAssesmentTerm(AssesmentTermVM cvm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var assesmentTerm = new AssesmentTerm
            {
                TermId = Guid.NewGuid(), // Generate a new GUID for TermId
                AssesmentId = cvm.AssesmentId,
                Term = cvm.Term,
                Weighting = cvm.Weighting,
                SubjectId = cvm.SubjectId
            };

            try
            {
                var success = await _termRepository.AddAssesmentTermAsync(assesmentTerm);
                if (!success)
                {
                    return BadRequest("Failed to add assessment term.");
                }
                return Ok(assesmentTerm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to add assessment term.");
                return BadRequest($"Internal Server Error: {ex.Message}");
            }
        }


        [HttpGet]
        [Route("GetAllTerms")]
        public async Task<IActionResult> GetAllTerms()
        {
            try
            {
                var terms = await _termRepository.GetAllTermsAsync();
                return Ok(terms);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching assessment terms.");
                return StatusCode(500, "Internal Server Error: Data is null or unavailable.");
            }
        }

        [HttpGet]
        [Route("GetTermById/{termId}")]
        public async Task<IActionResult> GetTermById(Guid termId)
        {
            try
            {
                var term = await _termRepository.GetTermByIdAsync(termId);
                if (term == null) return NotFound("The assesment term does not exist");
                return Ok(term);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve assessment term.");
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }

        [HttpPut]
        [Route("EditTerm/{termId}")]
        public async Task<ActionResult<AssesmentTermVM>> EditTerm(Guid termId, AssesmentTermVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var existingTerm = await _termRepository.GetTermByIdAsync(termId);
                if (existingTerm == null) return NotFound("The assesment term does not exist");

                existingTerm.AssesmentId = model.AssesmentId;
                existingTerm.Term = model.Term;
                existingTerm.Weighting = model.Weighting;
                existingTerm.SubjectId = model.SubjectId;

                var success = await _termRepository.UpdateAssesmentTermAsync(existingTerm);
                if (success)
                {
                    return Ok(existingTerm);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to edit assessment term.");
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }

            return BadRequest("Your request is invalid.");
        }

        [HttpDelete]
        [Route("DeleteTerm/{termId}")]
        public async Task<IActionResult> DeleteTerm(Guid termId)
        {
            try
            {
                var existingTerm = await _termRepository.GetTermByIdAsync(termId);
                if (existingTerm == null) return NotFound("The assesment term does not exist");

                var success = await _termRepository.DeleteAssesmentTermAsync(termId);
                if (success) return Ok(existingTerm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete assessment term.");
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }

            return BadRequest("Your request is invalid");
        }

    }
}
