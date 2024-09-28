using EduProfileAPI.Repositories.Implementation;
using EduProfileAPI.Repositories.Interfaces;
using EduProfileAPI.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EduProfileAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchoolEventController : ControllerBase
    {
        private readonly ISchoolEventRepo _schoolEventRepo;

        public SchoolEventController(ISchoolEventRepo schoolEventRepo)
        {
            _schoolEventRepo = schoolEventRepo;
        }

        [HttpPost]
        [Route("CreateSchoolEvent")]
        public async Task<IActionResult> CreateSchoolEvent([FromBody] CreateSchoolEventViewModel model, [FromQuery] Guid userId)
        {
            try
            {
                var schoolEvent = await _schoolEventRepo.CreateSchoolEventAsync(model, userId);  // Include userId for audit

                if (schoolEvent == null)
                    return NotFound("School Event not created. Not Permitted");

                return Ok(schoolEvent);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("GetAllSchoolEvents")]
        public async Task<IActionResult> GetAllSchoolEvents()
        {
            try
            {
                var schoolEvents = await _schoolEventRepo.GetAllSchoolEvents();

                if (schoolEvents == null)
                    return NotFound("No School Events found");

                return Ok(schoolEvents);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("GetSchoolEvent/{eventId}")] // returns a specific merit
        public async Task<IActionResult> GetSchoolEventAsync(Guid eventId)
        {
            try
            {
                var results = await _schoolEventRepo.GetSchoolEventAsync(eventId);

                if (results == null) return NotFound("event does not exist");

                return Ok(results);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut]
        [Route("EditSchoolEvent/{eventId}")]
        public async Task<ActionResult<CreateSchoolEventViewModel>> EditSchoolEvent(Guid eventId, [FromBody] CreateSchoolEventViewModel model, [FromQuery] Guid userId)
        {
            try
            {
                var existingEvent = await _schoolEventRepo.GetSchoolEventAsync(eventId);
                if (existingEvent == null) return NotFound($"The event does not exist");

                // Update event
                existingEvent.EmployeeId = model.EmployeeId;
                existingEvent.EventName = model.EventName;
                existingEvent.EventType = model.EventType;
                existingEvent.EventDate = model.EventDate;
                existingEvent.EventTime = model.EventTime;
                existingEvent.EventLocation = model.EventLocation;
                existingEvent.EventDescription = model.EventDescription;
                existingEvent.ContactPerson = model.ContactPerson;
                existingEvent.ContactEmail = model.ContactEmail;
                existingEvent.ContactPhoneNumber = model.ContactPhoneNumber;

                await _schoolEventRepo.UpdateSchoolEventAsync(existingEvent, userId);  // Include userId for audit
                await _schoolEventRepo.SaveChangesAsync();

                return Ok(existingEvent);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete]
        [Route("DeleteSchoolEvent/{eventId}")]
        public async Task<IActionResult> DeleteSchoolEvent(Guid eventId, [FromQuery] Guid userId)
        {
            try
            {
                var existingEvent = await _schoolEventRepo.GetSchoolEventAsync(eventId);
                if (existingEvent == null) return NotFound($"The event does not exist");

                await _schoolEventRepo.DeleteSchoolEventAsync(existingEvent, userId);  // Include userId for audit
                await _schoolEventRepo.SaveChangesAsync();

                return Ok(existingEvent);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
