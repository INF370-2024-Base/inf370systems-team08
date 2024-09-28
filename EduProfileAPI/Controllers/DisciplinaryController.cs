using EduProfileAPI.Models;
using EduProfileAPI.Repositories.Interfaces;
using EduProfileAPI.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace EduProfileAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DisciplinaryController : ControllerBase
    {
        private readonly IDisciplinaryRepository _disciplinaryRepository;
        private readonly ILogger<DisciplinaryController> _logger;
        public DisciplinaryController(IDisciplinaryRepository disciplinaryRepository, ILogger<DisciplinaryController> logger)
        {
            _disciplinaryRepository = disciplinaryRepository;
            _logger = logger;
        }

        [HttpGet]
        [Route("GetAllDisciplinaries")]
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
        [Route("GetDisciplinary/{disciplinaryId}")]
        public async Task<IActionResult> GetDisciplinaryAsync(Guid disciplinaryId)
        {
            try
            {
                var result = await _disciplinaryRepository.GetDisciplinaryAsync(disciplinaryId);
                if (result == null)
                    return NotFound("Disciplinary does not exist");

                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }

        [HttpPost]
        [Route("AddDisciplinary")]
        public async Task<IActionResult> AddDisciplinary([FromBody] CreateDisciplinaryVM cvm, [FromQuery] Guid userId)
        {
            var disciplinary = new Disciplinary
            {
                DisciplinaryTypeId = cvm.DisciplinaryTypeId,
                EmployeeId = cvm.EmployeeId,
                StudentId = cvm.StudentId,
                Reason = cvm.Reason,
                ParentContacted = cvm.ParentContacted,
                DisciplinaryDuration = cvm.DisciplinaryDuration,
                IssueDate = cvm.IssueDate
            };

            try
            {
                await _disciplinaryRepository.AddDisciplinaryAsync(disciplinary, userId);  // Include userId for audit
                await _disciplinaryRepository.SaveChangesAsync();
                return Ok(disciplinary);
            }
            catch (Exception)
            {
                return BadRequest("Invalid transaction");
            }
        }

        [HttpPut]
        [Route("EditDisciplinary/{disciplinaryId}")]
        public async Task<IActionResult> EditDisciplinary(Guid disciplinaryId, [FromBody] CreateDisciplinaryVM disciplinaryModel, [FromQuery] Guid userId)
        {
            try
            {
                var existingDisciplinary = await _disciplinaryRepository.GetDisciplinaryAsync(disciplinaryId);
                if (existingDisciplinary == null)
                    return NotFound("The disciplinary does not exist");
                var updateDisciplinary = new Disciplinary
                {
                    DisciplinaryId = disciplinaryId,
                    EmployeeId = disciplinaryModel.EmployeeId,
                    DisciplinaryTypeId = disciplinaryModel.DisciplinaryTypeId,
                    StudentId = disciplinaryModel.StudentId,
                    Reason = disciplinaryModel.Reason,
                    ParentContacted = disciplinaryModel.ParentContacted,
                    DisciplinaryDuration = disciplinaryModel.DisciplinaryDuration,
                    IssueDate = disciplinaryModel.IssueDate
                };


                await _disciplinaryRepository.UpdateDisciplinaryAsync(updateDisciplinary, existingDisciplinary, userId);  // Include userId for audit
                await _disciplinaryRepository.SaveChangesAsync();
                return Ok(existingDisciplinary);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while editing disciplinary with ID: {DisciplinaryId}", disciplinaryId);
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }

        [HttpDelete]
        [Route("DeleteDisciplinary/{disciplinaryId}")]
        public async Task<IActionResult> DeleteDisciplinary(Guid disciplinaryId, [FromQuery] Guid userId)
        {
            try
            {
                var existingDisciplinary = await _disciplinaryRepository.GetDisciplinaryAsync(disciplinaryId);
                if (existingDisciplinary == null)
                    return NotFound("The disciplinary does not exist");

                await _disciplinaryRepository.DeleteDisciplinaryAsync(existingDisciplinary, userId);  // Include userId for audit
                await _disciplinaryRepository.SaveChangesAsync();
                return Ok(existingDisciplinary);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }
    }
}
