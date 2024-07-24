using EduProfileAPI.Repositories.Interfaces;
using EduProfileAPI.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EduProfileAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchoolEventController : ControllerBase
    {
        private readonly ISchoolEventRepo _schoolEventRepo;

        public SchoolEventController(ISchoolEventRepo schoolEventRepo)
        {
            _schoolEventRepo = schoolEventRepo;
        }

        [HttpPost]
        public async Task<IActionResult> CreateSchoolEvent([FromBody] CreateSchoolEventViewModel model)
        {
            try
            {
                var schoolEvent = await _schoolEventRepo.CreateSchoolEvent(model);

                if (schoolEvent == null)
                    return NotFound("School Event not created. Not Permitted");

                return Ok(schoolEvent);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
