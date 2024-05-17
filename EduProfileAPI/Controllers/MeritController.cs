using EduProfileAPI.Models;
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
                var results = await _meritRepository.GetAllMeritAsync();
                return Ok(results);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }

        [HttpGet]
        [Route("GetMerit/{meritId}")] //returns a specific course 
        public async Task<IActionResult> GetMeritAsync(int meritId)
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
        public async Task<IActionResult> AddMerit(CreateMeritVM cvm)
        {
            var merit = new Merit { MeritTypeId = cvm.MeritTypeId,EmployeeId = cvm.EmployeeId  ,MeritName = cvm.MeritName, MeritDescription = cvm.MeritDescription };

            try
            {
                _meritRepository.Add(merit);
                await _meritRepository.SaveChangesAsync();
            }
            catch (Exception)
            {
                return BadRequest("Invalid transaction");
            }

            return Ok(merit);
        }

        [HttpPut]
        [Route("EditMerit/{meritId}")]
        public async Task<ActionResult<CreateMeritVM>> EditMerit(int meritId, CreateMeritVM meritModel)
        {
            try
            {
                var existingMerit = await _meritRepository.GetMeritAsync(meritId);
                if (existingMerit == null) return NotFound($"The merit does not exist");
                existingMerit.EmployeeId = meritModel.EmployeeId;
                existingMerit.MeritTypeId = meritModel.MeritTypeId;
                existingMerit.MeritName = meritModel.MeritName;
                existingMerit.MeritDescription = meritModel.MeritDescription;

                if (await _meritRepository.SaveChangesAsync())
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
        [Route("DeleteMerit/{meritId}")]
        public async Task<IActionResult> MeritCourse(int meritId)
        {
            try
            {
                var existingMerit = await _meritRepository.GetMeritAsync(meritId);
                if (existingMerit == null) return NotFound($"The merit does not exist");
                _meritRepository.Delete(existingMerit);

                if (await _meritRepository.SaveChangesAsync()) return Ok(existingMerit);
            }
            catch (Exception)
            {

                return StatusCode(500, "Internal Server Error. Please contact support.");
            }

            return BadRequest("Your request is invalid");
        }
    }

}
