using EduProfileAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EduProfileAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeritTypeController : ControllerBase
    {
        private readonly IMeritType _meritTypeRepository;

        public MeritTypeController(IMeritType meritTypeRepository)
        {
            _meritTypeRepository = meritTypeRepository;
        }

        [HttpGet]
        [Route("GetAllMeritTypes")] //returns a list of merits
        public async Task<IActionResult> GetAllMeritTypes()
        {
            try
            {
                var results = await _meritTypeRepository.GetAllMeritTypesAsync();
                return Ok(results);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }

        [HttpGet]
        [Route("GetMeritType/{meritTypeId}")] // returns a specific merit
        public async Task<IActionResult> GetMeritType(Guid meritTypeId)
        {
            try
            {
                var results = await _meritTypeRepository.GetMeritTypeAsync(meritTypeId);

                if (results == null) return NotFound("Merit Type does not exist");

                return Ok(results);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }
    }
}
