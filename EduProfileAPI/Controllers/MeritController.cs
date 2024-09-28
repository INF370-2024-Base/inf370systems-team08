using EduProfileAPI.Models;
using EduProfileAPI.Repositories.Interfaces;
using EduProfileAPI.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EduProfileAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeritController : ControllerBase
    {
        private readonly IMeritRepository _meritRepository;

        public MeritController(IMeritRepository meritRepository)
        {
            _meritRepository = meritRepository;
        }

        [HttpGet]
        [Route("GetAllMerits")] //returns a list of merits
        public async Task<IActionResult> GetAllMerits()
        {
            try
            {
                var results = await _meritRepository.GetAllMeritsAsync();
                return Ok(results);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }

        [HttpGet]
        [Route("GetMerit/{meritId}")] // returns a specific merit
        public async Task<IActionResult> GetMeritAsync(Guid meritId)
        {
            try
            {
                var results = await _meritRepository.GetMeritAsync(meritId);

                if (results == null) return NotFound("Merit does not exist");

                return Ok(results);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }
        [HttpPost]
        [Route("AddMerit")]
        public async Task<IActionResult> AddMerit([FromBody] CreateMeritVM cvm, [FromQuery] Guid userId)
        {
            var merit = new Merit
            {
                MeritTypeId = cvm.MeritTypeId,
                EmployeeId = cvm.EmployeeId,
                StudentId = cvm.StudentId,
                MeritName = cvm.MeritName,
                MeritDescription = cvm.MeritDescription
            };

            try
            {
                await _meritRepository.AddMeritAsync(merit, userId);  // Ensure AddMeritAsync accepts userId
                await _meritRepository.SaveChangesAsync();
                return Ok(merit);
            }
            catch (Exception)
            {
                return BadRequest("Invalid transaction");
            }
        }



        [HttpPut]
        [Route("EditMerit/{meritId}")]
        public async Task<ActionResult<CreateMeritVM>> EditMerit(Guid meritId, [FromBody] CreateMeritVM meritModel, [FromQuery] Guid userId)
        {
            try
            {
                var existingMerit = await _meritRepository.GetMeritAsync(meritId);
                if (existingMerit == null) return NotFound($"The merit does not exist");

                // Create an updated merit object
                var updatedMerit = new Merit
                {
                    MeritId = meritId,
                    EmployeeId = meritModel.EmployeeId,
                    MeritTypeId = meritModel.MeritTypeId,
                    StudentId = meritModel.StudentId,
                    MeritName = meritModel.MeritName,
                    MeritDescription = meritModel.MeritDescription
                };

                // Log the audit trail and perform the update in the repository
                await _meritRepository.UpdateMeritAsync(updatedMerit, existingMerit, userId);  // Log changes
                await _meritRepository.SaveChangesAsync();

                return Ok(updatedMerit);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }


        [HttpDelete]
        [Route("DeleteMerit/{meritId}")]
        public async Task<IActionResult> DeleteMerit(Guid meritId, [FromQuery] Guid userId)
        {
            try
            {
                var existingMerit = await _meritRepository.GetMeritAsync(meritId);
                if (existingMerit == null) return NotFound($"The merit does not exist");

                // Perform the delete in the repository
                await _meritRepository.DeleteMeritAsync(existingMerit, userId);  // Log deletion
                await _meritRepository.SaveChangesAsync();

                return Ok(existingMerit);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }

    }

}
