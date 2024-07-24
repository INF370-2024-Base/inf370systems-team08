using EduProfileAPI.Repositories.Interfaces;
using EduProfileAPI.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EduProfileAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EarlyReleasesController : ControllerBase
    {
        private readonly IEarlyReleasesRepo _earlyReleasesRepo;

        public EarlyReleasesController(IEarlyReleasesRepo earlyReleasesRepo)
        {
            _earlyReleasesRepo = earlyReleasesRepo;
        }

        [HttpPost]
        public async Task<IActionResult> CreateEarlyRelease([FromBody] CreateEarlyReleaseVM model)
        {
            try
            {
                var earlyRelease = await _earlyReleasesRepo.CreateEarlyRelease(model);

                if (earlyRelease == null)
                    return NotFound("Student or Parent not found.");

                return Ok(earlyRelease);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
