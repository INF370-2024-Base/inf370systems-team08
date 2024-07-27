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
        public async Task<IActionResult> AddProcedure(MaintenanceProcedureVM cvm)
        {
            var procedure = new MaintenanceProcedure { EmployeeId = cvm.EmployeeId, CompletionDate = cvm.CompletionDate, Description = cvm.Description, Comments = cvm.Comments, Costs = cvm.Costs };

            try
            {
                _proRepository.Add(procedure);
                await _proRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest($"Internal Server Error: {ex.Message}");
            }

            return Ok(procedure);
        }

        [HttpPut]
        [Route("EditProcedure/{maintenanceProId}")]
        public async Task<ActionResult<MaintenanceProcedureVM>> EditProcedure(Guid maintenanceProId, MaintenanceProcedureVM mvm)
        {
            try
            {
                var existingPro = await _proRepository.GetProcedureAsync(maintenanceProId);
                if (existingPro == null) return NotFound($"The procedure does not exist");
                existingPro.EmployeeId = mvm.EmployeeId;
                existingPro.Description = mvm.Description;
                existingPro.CompletionDate = mvm.CompletionDate;
                existingPro.Comments = mvm.Comments;
                existingPro.Costs = mvm.Costs;


                if (await _proRepository.SaveChangesAsync())
                {
                    return Ok(existingPro);
                }

            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");

            }
            return BadRequest("Your request is invalid.");


        }

        [HttpDelete]
        [Route("DeleteProcedure/{maintenanceProId}")]
        public async Task<IActionResult> DeleteProcedure(Guid maintenanceProId)
        {
            try
            {
                var existingPro = await _proRepository.GetProcedureAsync(maintenanceProId);
                if (existingPro == null) return NotFound($"The request does not exist");
                _proRepository.Delete(existingPro);

                if (await _proRepository.SaveChangesAsync()) return Ok(existingPro);
            }
            catch (Exception)
            {

                return StatusCode(500, "Internal Server Error. Please contact support.");
            }

            return BadRequest("Your request is invalid");
        }
    }
}
