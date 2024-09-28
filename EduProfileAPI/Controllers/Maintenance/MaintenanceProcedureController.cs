using EduProfileAPI.Models.Maintenance;
using EduProfileAPI.Repositories.Interfaces.Maintenance;
using EduProfileAPI.ViewModels.Maintenance;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EduProfileAPI.Controllers.Maintenance
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaintenanceProcedureController : ControllerBase
    {
        private readonly IMaintenanceProcedure _proRepository;

        public MaintenanceProcedureController(IMaintenanceProcedure proRepository)
        {
            _proRepository = proRepository;
        }

        [HttpGet]
        [Route("GetAllProcedures")] //returns a list of merits
        public async Task<IActionResult> GetAllProcedures()
        {
            try
            {
                var results = await _proRepository.GetAllProceduresAsync();
                return Ok(results);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("GetProcedure/{maintenanceProId}")]
        public async Task<IActionResult> GetProcedureAsync(Guid maintenanceProId)
        {
            try
            {
                var results = await _proRepository.GetProcedureAsync(maintenanceProId);

                if (results == null) return NotFound("Procedure does not exist");

                return Ok(results);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }

        [HttpPost]
        [Route("AddProcedure")]
        public async Task<IActionResult> AddProcedure([FromBody] MaintenanceProcedureVM cvm, [FromQuery] Guid userId)
        {
            var procedure = new MaintenanceProcedure
            {
                EmployeeId = cvm.EmployeeId,
                CompletionDate = cvm.CompletionDate,
                Description = cvm.Description,
                Comments = cvm.Comments,
                Costs = cvm.Costs
            };

            try
            {
                await _proRepository.AddProcedureAsync(procedure, userId);
                return Ok(procedure);
            }
            catch (Exception ex)
            {
                return BadRequest($"Internal Server Error: {ex.Message}");
            }
        }

        [HttpPut]
        [Route("EditProcedure/{maintenanceProId}")]
        public async Task<ActionResult<MaintenanceProcedureVM>> EditProcedure(Guid maintenanceProId, [FromBody] MaintenanceProcedureVM mvm, [FromQuery] Guid userId)
        {
            try
            {
                var existingPro = await _proRepository.GetProcedureAsync(maintenanceProId);
                if (existingPro == null) return NotFound($"The procedure does not exist");

                // Create a backup of the old values for audit trail logging
                var oldProcedure = existingPro;

                existingPro.EmployeeId = mvm.EmployeeId;
                existingPro.Description = mvm.Description;
                existingPro.CompletionDate = mvm.CompletionDate;
                existingPro.Comments = mvm.Comments;
                existingPro.Costs = mvm.Costs;

                await _proRepository.UpdateProcedureAsync(existingPro, oldProcedure, userId);
                return Ok(existingPro);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }

        [HttpDelete]
        [Route("DeleteProcedure/{maintenanceProId}")]
        public async Task<IActionResult> DeleteProcedure(Guid maintenanceProId, [FromQuery] Guid userId)
        {
            try
            {
                var existingPro = await _proRepository.GetProcedureAsync(maintenanceProId);
                if (existingPro == null) return NotFound($"The request does not exist");

                await _proRepository.DeleteProcedureAsync(existingPro, userId);
                return Ok(existingPro);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }
    }
}
