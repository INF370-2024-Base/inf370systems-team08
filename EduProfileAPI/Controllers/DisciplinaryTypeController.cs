using EduProfileAPI.Models;
using EduProfileAPI.Repositories.Interfaces;
using EduProfileAPI.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace EduProfileAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DisciplinaryTypeController : ControllerBase
    {
        private readonly IDisciplinaryTypeRepository _disciplinaryTypeRepository;
        private readonly ILogger<DisciplinaryTypeController> _logger;

        public DisciplinaryTypeController(IDisciplinaryTypeRepository disciplinaryTypeRepository, ILogger<DisciplinaryTypeController> logger)
        {
            _disciplinaryTypeRepository = disciplinaryTypeRepository;
            _logger = logger;
        }

        [HttpGet]
        [Route("GetAllDisciplinaryTypes")]
        public async Task<IActionResult> GetAllDisciplinaryTypes()
        {
            try
            {
                var results = await _disciplinaryTypeRepository.GetAllDisciplinaryTypesAsync();
                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting all disciplinary types.");
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }

        [HttpGet]
        [Route("GetDisciplinaryType/{disciplinaryTypeId}")]
        public async Task<IActionResult> GetDisciplinaryTypeAsync(Guid disciplinaryTypeId)
        {
            try
            {
                var result = await _disciplinaryTypeRepository.GetDisciplinaryTypeAsync(disciplinaryTypeId);
                if (result == null)
                    return NotFound("Disciplinary type does not exist");

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting disciplinary type with ID: {DisciplinaryTypeId}", disciplinaryTypeId);
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }

        [HttpPost]
        [Route("AddDisciplinaryType")]
        public async Task<IActionResult> AddDisciplinaryType(CreateDisciplinaryTypeVM cvm)
        {
            var disciplinaryType = new DisciplinaryType
            {
                DisciplinaryTypeName = cvm.DisciplinaryTypeName,
                DisciplinaryTypeDescription = cvm.DisciplinaryTypeDescription
            };

            try
            {
                _disciplinaryTypeRepository.Add(disciplinaryType);
                await _disciplinaryTypeRepository.SaveChangesAsync();
                return Ok(disciplinaryType);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding disciplinary type.");
                return BadRequest("Invalid transaction");
            }
        }

        [HttpPut]
        [Route("EditDisciplinaryType/{disciplinaryTypeId}")]
        public async Task<IActionResult> EditDisciplinaryType(Guid disciplinaryTypeId, CreateDisciplinaryTypeVM disciplinaryTypeModel)
        {
            try
            {
                var existingDisciplinaryType = await _disciplinaryTypeRepository.GetDisciplinaryTypeAsync(disciplinaryTypeId);
                if (existingDisciplinaryType == null)
                    return NotFound("The disciplinary type does not exist");

                existingDisciplinaryType.DisciplinaryTypeName = disciplinaryTypeModel.DisciplinaryTypeName;
                existingDisciplinaryType.DisciplinaryTypeDescription = disciplinaryTypeModel.DisciplinaryTypeDescription;

                if (await _disciplinaryTypeRepository.SaveChangesAsync())
                    return Ok(existingDisciplinaryType);
                else
                    return StatusCode(500, "Unable to save changes to the database.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while editing disciplinary type with ID: {DisciplinaryTypeId}", disciplinaryTypeId);
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }

        [HttpDelete]
        [Route("DeleteDisciplinaryType/{disciplinaryTypeId}")]
        public async Task<IActionResult> DeleteDisciplinaryType(Guid disciplinaryTypeId)
        {
            try
            {
                var existingDisciplinaryType = await _disciplinaryTypeRepository.GetDisciplinaryTypeAsync(disciplinaryTypeId);
                if (existingDisciplinaryType == null)
                    return NotFound("The disciplinary type does not exist");

                _disciplinaryTypeRepository.Delete(existingDisciplinaryType);

                if (await _disciplinaryTypeRepository.SaveChangesAsync())
                    return Ok(existingDisciplinaryType);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting disciplinary type with ID: {DisciplinaryTypeId}", disciplinaryTypeId);
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }

            return BadRequest("Your request is invalid.");
        }
    }
}
