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

        public StudentIncidentController(IStudentIncidentRepository studentincidentrepository )
        {
            _Repository = studentincidentrepository;
        }

        // GET: api/StudentIncidents
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentIncident>>> GetStudentIncidents()
        {
            var incidents = await _Repository.GetAllAsync();
            return Ok(incidents);
        }

        // GET: api/StudentIncidents/5
        [HttpGet("{id}")]
        public async Task<ActionResult<StudentIncident>> GetStudentIncident(Guid id)
        {
            var studentIncident = await _Repository.GetByIdAsync(id);

            if (studentIncident == null)
            {
                return NotFound();
            }

            return studentIncident;
        }

        // PUT: api/StudentIncidents/5
        [HttpPut("{id}")]
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
        public async Task<ActionResult<StudentIncident>> AddStudentIncident(StudentIncident studentIncident)
        {
            await _Repository.AddAsync(studentIncident);
            return CreatedAtAction("GetStudentIncident", new { id = studentIncident.IncidentId }, studentIncident);
        }

        // DELETE: api/StudentIncidents/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudentIncident(Guid id)
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
