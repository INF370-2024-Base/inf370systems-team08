using EduProfileAPI.Models;
using EduProfileAPI.Repositories.Implementation;
using EduProfileAPI.Repositories.Interfaces;
using EduProfileAPI.ViewModels;
using EduProfileAPI.ViewModels.Maintenance;
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

        [HttpPost]
        [Route("AddMeritType")]
        public async Task<IActionResult> AddMeritType(MeritTypeVm cvm)
        {
            var merit = new MeritType { MeritTypeName = cvm.MeritTypeName };

            try
            {
                _meritTypeRepository.Add(merit);
                await _meritTypeRepository.SaveChangesAsync();
            }
            catch (Exception)
            {
                return BadRequest("Invalid transaction");
            }

            return Ok(merit);
        }

        [HttpPut]
        [Route("EditMeritType/{meritTypeId}")]
        public async Task<ActionResult<MeritTypeVm>> EditMeritType(Guid meritTypeId, MeritTypeVm model)
        {
            try
            {
                var existingMerit = await _meritTypeRepository.GetMeritTypeAsync(meritTypeId);
                if (existingMerit == null) return NotFound($"The merit type does not exist");
                existingMerit.MeritTypeName = model.MeritTypeName;


                if (await _meritTypeRepository.SaveChangesAsync())
                {
                    return Ok(existingMerit);
                }

            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");

            }
            return BadRequest("Your request is invalid.");


        }

        [HttpDelete]
        [Route("DeleteMeritType/{meritTypeId}")]
        public async Task<IActionResult> DeleteMeritType(Guid meritTypeId)
        {
            try
            {
                var existingMerit = await _meritTypeRepository.GetMeritTypeAsync(meritTypeId);
                if (existingMerit == null) return NotFound($"The merit Type does not exist");
                _meritTypeRepository.Delete(existingMerit);

                if (await _meritTypeRepository.SaveChangesAsync()) return Ok(existingMerit);
            }
            catch (Exception)
            {

                return StatusCode(500, "Internal Server Error. Please contact support.");
            }

            return BadRequest("Your request is invalid");
        }
    }
}
