using EduProfileAPI.Repositories.Interfaces.Maintenance;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EduProfileAPI.Controllers.Maintenance
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaintenanceStatusController : ControllerBase
    {
        private readonly IMaintenanceStatus _statusRepository;

        public MaintenanceStatusController(IMaintenanceStatus statusRepository)
        {
            _statusRepository = statusRepository;
        }

        [HttpGet]
        [Route("GetAllStatus")]
        public async Task<IActionResult> GetAllStatus()
        {
            try
            {
                var results = await _statusRepository.GetAllStatusAsync();
                return Ok(results);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }
    }
}
