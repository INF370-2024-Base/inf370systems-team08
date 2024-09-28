using EduProfileAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EduProfileAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuditTrailController : ControllerBase
    {
        private readonly IAuditTrail _auditTrailRepository;

        public AuditTrailController(IAuditTrail auditTrailRepository)
        {
            _auditTrailRepository = auditTrailRepository;
        }

        [HttpGet]
        [Route("GetAllAuditTrail")]
        public async Task<IActionResult> GetAllAuditTrail()
        {
            try
            {
                var results = await _auditTrailRepository.GetAllAuditTrailAsync();
                return Ok(results);  // Return the audit trail results to the frontend
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error. Please contact support. Error details: {ex.Message}");
            }
        }
    }
}
