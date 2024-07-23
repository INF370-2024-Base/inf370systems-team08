using EduProfileAPI.Models;
using EduProfileAPI.Repositories.Interfaces;
using EduProfileAPI.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EduProfileAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DisciplinaryController : ControllerBase
    {
        private readonly IDisciplinaryRepository _disciplinaryRepository;

        public DisciplinaryController(IDisciplinaryRepository disciplinaryRepository)
        {
            _disciplinaryRepository = disciplinaryRepository;
        }

        [HttpGet]
        [Route("GetAllDisciplinaries")] //returns a list of disciplinaries
        public async Task<IActionResult> GetAllDisciplinaries()
        {
            try
            {
                var results = await _disciplinaryRepository.GetAllDisciplinariesAsync();
                return Ok(results);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }

        [HttpGet]
        [Route("GetDisciplinary/{disciplinaryId}")] // returns a specific disciplinary
        public async Task<IActionResult> GetDisciplinaryAsync(Guid disciplinaryId)
        {
            try
            {
                var results = await _disciplinaryRepository.GetDisciplinaryAsync(disciplinaryId);

                if (results == null) return NotFound("Disciplinary does not exist");

                return Ok(results);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }

        [HttpPost]
        [Route("AddDisciplinary")]
        public async Task<IActionResult> AddDisciplinary(CreateDisciplinaryVM cvm)
        {
            var disciplinary = new Disciplinary { DisciplinaryTypeId = cvm.DisciplinaryTypeId, EmployeeId = cvm.EmployeeId, StudentId = cvm.StudentId, DisciplinaryName = cvm.DisciplinaryName, DisciplinaryDescription = cvm.DisciplinaryDescription };

            try
            {
                _disciplinaryRepository.Add(disciplinary);
                await _disciplinaryRepository.SaveChangesAsync();
            }
            catch (Exception)
            {
                return BadRequest("Invalid transaction");
            }

            return Ok(disciplinary);
        }

        [HttpPut]
        [Route("EditDisciplinary/{disciplinaryId}")]
        public async Task<ActionResult<CreateDisciplinaryVM>> EditDisciplinary(Guid disciplinaryId, CreateDisciplinaryVM disciplinaryModel)
        {
            try
            {
                var existingDisciplinary = await _disciplinaryRepository.GetDisciplinaryAsync(disciplinaryId);
                if (existingDisciplinary == null) return NotFound("The disciplinary does not exist");
                existingDisciplinary.EmployeeId = disciplinaryModel.EmployeeId;
                existingDisciplinary.DisciplinaryTypeId = disciplinaryModel.DisciplinaryTypeId;
                existingDisciplinary.StudentId = disciplinaryModel.StudentId;
                existingDisciplinary.DisciplinaryName = disciplinaryModel.DisciplinaryName;
                existingDisciplinary.DisciplinaryDescription = disciplinaryModel.DisciplinaryDescription;

                if (await _disciplinaryRepository.SaveChangesAsync())
                {
                    return Ok(existingDisciplinary);
                }

            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
            return BadRequest("Your request is invalid.");
        }

        [HttpDelete]
        [Route("DeleteDisciplinary/{disciplinaryId}")]
        public async Task<IActionResult> DeleteDisciplinary(Guid disciplinaryId)
        {
            try
            {
                var existingDisciplinary = await _disciplinaryRepository.GetDisciplinaryAsync(disciplinaryId);
                if (existingDisciplinary == null) return NotFound("The disciplinary does not exist");
                _disciplinaryRepository.Delete(existingDisciplinary);

                if (await _disciplinaryRepository.SaveChangesAsync()) return Ok(existingDisciplinary);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }

            return BadRequest("Your request is invalid");
        }
    }
}
