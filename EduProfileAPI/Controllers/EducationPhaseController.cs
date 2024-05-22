using EduProfileAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EduProfileAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EducationPhaseController : ControllerBase
    {
        private readonly IEducationPhaseRepository _educationPhaseRepo;

        public EducationPhaseController(IEducationPhaseRepository educationPhaseRepo)
        {
            _educationPhaseRepo = educationPhaseRepo;
        }

        [HttpGet]
        [Route("GetAllEducationPhases")]
        public async Task<IActionResult> GetAllEducationPhasesAsync()
        {
            try
            {
                var results = await _educationPhaseRepo.GetAllEducationPhasesAsync();
                return Ok(results);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Database Failure: {ex.Message}");
            }
        }
    }
}
