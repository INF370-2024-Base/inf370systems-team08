using EduProfileAPI.Models;
using EduProfileAPI.Models.Maintenance;
using EduProfileAPI.Repositories.Interfaces.Maintenance;
using EduProfileAPI.ViewModels;
using EduProfileAPI.ViewModels.Maintenance;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework.Internal.Execution;

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

        [HttpGet]
        [Route("GetMaintenanceType/{maintenanceTypeId}")] 
        public async Task<IActionResult> GetMaintenanceType(Guid maintenanceTypeId)
        {
            try
            {
                var results = await _typeRepository.GetMaintenanceType(maintenanceTypeId);

                if (results == null) return NotFound("Maintenance type does not exist");

                return Ok(results);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }

        [HttpPost]
        [Route("AddMaintenanceType")]
        public async Task<IActionResult> AddMaintenanceType(MaintenanceTypeVM cvm)
        {
            var type = new MaintenanceType { Description = cvm.Description };

            try
            {
                _typeRepository.Add(type);
                await _typeRepository.SaveChangesAsync();
            }
            catch (Exception)
            {
                return BadRequest("Invalid transaction");
            }

            return Ok(type);
        }

        [HttpPut]
        [Route("EditMaintenanceType/{maintenanceTypeId}")]
        public async Task<ActionResult<MaintenanceTypeVM>> EditMaintenanceType(Guid maintenanceTypeId, MaintenanceTypeVM model)
        {
            try
            {
                var existing = await _typeRepository.GetMaintenanceType(maintenanceTypeId);
                if (existing == null) return NotFound($"The maintenance does not exist");
                existing.Description = model.Description;
        

                if (await _typeRepository.SaveChangesAsync())
                {
                    return Ok(existing);
                }

            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");

            }
            return BadRequest("Your request is invalid.");


        }

        [HttpDelete]
        [Route("DeleteMaintenanceType/{maintenanceTypeId}")]
        public async Task<IActionResult> DeleteMaintenanceType(Guid maintenanceTypeId)
        {
            try
            {
                var existing = await _typeRepository.GetMaintenanceType(maintenanceTypeId);
                if (existing == null) return NotFound($"The maintenance does not exist");
                _typeRepository.Delete(existing);

                if (await _typeRepository.SaveChangesAsync()) return Ok(existing);
            }
            catch (Exception)
            {

                return StatusCode(500, "Internal Server Error. Please contact support.");
            }

            return BadRequest("Your request is invalid");
        }
    }

}
