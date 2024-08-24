using Microsoft.AspNetCore.Mvc;
using EduProfileAPI.Models;
using EduProfileAPI.Repositories.Interfaces;
using EduProfileAPI.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace EduProfileAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentIncidentController : ControllerBase
    {
        private readonly IStudentIncidentRepository _Repository;

        public StudentIncidentController(IStudentIncidentRepository studentincidentrepository)
        {
            _Repository = studentincidentrepository;
        }

        // GET: api/StudentIncidents
        [HttpGet]
        [Route("GetIncidents")]
        public async Task<IActionResult> GetStudentIncidentsAsync()
        {
            var incidents = await _Repository.GetAllAsync();
            return Ok(incidents);
        }

        // GET: api/StudentIncidents/5
        [HttpGet]
        [Route("GetIncident/{id}")]
        public async Task<IActionResult> GetStudentIncidentAsync(Guid id)
        {
            try
            {
                var result = await _Repository.GetByIdAsync(id);
                if (result == null)
                {
                    return NotFound("Grade not found.");
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Database Failure: {ex.Message}");
            }
        }

        // PUT: api/StudentIncidents/5
        [HttpPut]
        [Route("Update/{id}")]
        public async Task<IActionResult> UpdateStudentIncident(Guid id, StudentIncident studentIncident)
        {
            if (id != studentIncident.IncidentId)
            {
                return BadRequest();
            }

            try
            {
                await _Repository.UpdateAsync(studentIncident);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _Repository.ExistsAsync(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/StudentIncidents
        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> AddStudentIncident([FromBody] StudentIncident studentIncident)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                //studentIncident.IncidentId = Guid.NewGuid();
                await _Repository.AddAsync(studentIncident);
                await _Repository.SaveChangesAsync(); // Ensure changes are saved to the database
                return CreatedAtAction("GetStudentIncident", new { id = studentIncident.IncidentId }, studentIncident);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error adding student incident: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        // DELETE: api/StudentIncidents/5
        [HttpDelete]
        [Route("Delete/{id}")]
        public async Task<IActionResult> DeleteStudentIncident(Guid id)
        {
            var result = await _Repository.DeleteAsync(id);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        // GET: api/StudentIncidents
        [HttpGet]
        [Route("GetIncidentTypes")]
        public async Task<IActionResult> GetStudentIncidentTypesAsync()
        {
            var types = await _Repository.GetAllTypesAsync();
            return Ok(types);
        }

        // GET: api/StudentIncidents/5
        [HttpGet]
        [Route("GetStudentIncidentTypeById/{id}")]
        public async Task<IActionResult> GetStudentIncidentTypeById(Guid id)
        {
            try
            {
                var result = await _Repository.GetByTypeIdAsync(id);
                if (result == null)
                {
                    return NotFound("incident not found.");
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Database Failure: {ex.Message}");
            }
        }

        [HttpPost("IncidentType")]
        public async Task<IActionResult> AddIncidentType([FromBody] IncidentType incidentType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _Repository.AddIncidentAsync(incidentType);
                await _Repository.SaveChangesAsync(); // Ensure changes are saved to the database
                return Ok(incidentType);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error adding incident type: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }

        }

        [HttpPut]
        [Route("UpdateIncidentType/{id}")]
        public async Task<IActionResult> UpdateIncidentType(Guid id, IncidentType incidentType)
        {
            if (id != incidentType.IncidentTypeId)
            {
                return BadRequest();
            }

            try
            {
                await _Repository.UpdateIncidentType(incidentType);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _Repository.ExistTypeAsync(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete]
        [Route("DeleteIncidentType/{id}")]
        public async Task<IActionResult> DeleteIncidentType(Guid id)
        {
            var result = await _Repository.DeleteAsync(id);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
