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

            [HttpGet("getstudentreleases/{studentId}")]
            public async Task<IActionResult> GetEarlyReleasesByStudentId(Guid studentId)
            {
                if (studentId == Guid.Empty)
                {
                    return BadRequest("Invalid Student ID.");
                }

                try
                {
                    var earlyReleases = await _earlyReleasesRepo.GetEarlyReleasesByStudentId(studentId);

                    if (earlyReleases == null || !earlyReleases.Any())
                    {
                        return NotFound($"No early release records found for Student ID: {studentId}");
                    }
                    return Ok(earlyReleases);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, ex);
                }
            }
        }
    }
