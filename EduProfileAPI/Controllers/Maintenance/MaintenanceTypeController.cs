using EduProfileAPI.Repositories.Interfaces.Maintenance;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EduProfileAPI.Controllers.Maintenance
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaintenanceTypeController : ControllerBase
    {
        private readonly IMaintenanceType _typeRepository;

        public MaintenanceTypeController(IMaintenanceType typeRepository)
        {
            _typeRepository = typeRepository;
        }

        [HttpGet]
        [Route("GetAllTypes")]
        public async Task<IActionResult> GetAllTypes()
        {
            try
            {
                var results = await _typeRepository.GetAllTypesAsync();
                return Ok(results);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }
    }
}
