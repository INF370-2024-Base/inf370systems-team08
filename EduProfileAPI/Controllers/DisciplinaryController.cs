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

        public DisciplinaryController(IDisciplinaryRepository disciplinaryRepository)
        {
            _disciplinaryRepository = disciplinaryRepository;
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
        public async Task<IActionResult> AddDisciplinary(CreateDisciplinaryVM cvm)
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
                _disciplinaryRepository.Add(disciplinary);
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
        public async Task<IActionResult> EditDisciplinary(Guid disciplinaryId, CreateDisciplinaryVM disciplinaryModel)
        {
            try
            {
                var existingDisciplinary = await _disciplinaryRepository.GetDisciplinaryAsync(disciplinaryId);
                if (existingDisciplinary == null)
                    return NotFound("The disciplinary does not exist");

                existingDisciplinary.EmployeeId = disciplinaryModel.EmployeeId;
                existingDisciplinary.DisciplinaryTypeId = disciplinaryModel.DisciplinaryTypeId;
                existingDisciplinary.StudentId = disciplinaryModel.StudentId;
                existingDisciplinary.Reason = disciplinaryModel.Reason;
                existingDisciplinary.ParentContacted = disciplinaryModel.ParentContacted;
                existingDisciplinary.DisciplinaryDuration = disciplinaryModel.DisciplinaryDuration;
                existingDisciplinary.IssueDate = disciplinaryModel.IssueDate;

                if (await _disciplinaryRepository.SaveChangesAsync())
                    return Ok(existingDisciplinary);
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
                if (existingDisciplinary == null)
                    return NotFound("The disciplinary does not exist");

                _disciplinaryRepository.Delete(existingDisciplinary);

                if (await _disciplinaryRepository.SaveChangesAsync())
                    return Ok(existingDisciplinary);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }

            return BadRequest("Your request is invalid.");
        }
    }
}
