using EduProfileAPI.Repositories.Interfaces;
using EduProfileAPI.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EduProfileAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactStudentParentController : ControllerBase
    {
        private readonly IContactStudentParent _contactStudentParentRepository;

        public ContactStudentParentController(IContactStudentParent contactStudentParentRepository)
        {
            _contactStudentParentRepository = contactStudentParentRepository;
        }

        // Endpoint to get parent details by student ID
        [HttpGet("{studentId}")]
        public async Task<IActionResult> GetParentDetails(Guid studentId)
        {
            var result = await _contactStudentParentRepository.GetParentDetailsByStudentId(studentId);
            if (result == null)
                return NotFound("Parent details not found.");

            return Ok(result);
        }

        // Endpoint to send a message to the parent with Audit Trail
        [HttpPost("send-message")]
        public async Task<IActionResult> SendMessage([FromBody] ContactStudentParentViewModel model, [FromQuery] Guid userId)
        {
            if (model == null || model.StudentId == Guid.Empty || model.ParentId == Guid.Empty || string.IsNullOrEmpty(model.Message))
            {
                return BadRequest("Invalid request data.");
            }

            try
            {
                var (success, responseDetails) = await _contactStudentParentRepository.SendMessageToParent(model, userId);
                if (!success)
                {
                    return StatusCode(500, $"Failed to send message. External service response: {responseDetails}");
                }

                return Ok("Message sent successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}\n{ex.StackTrace}");
            }
        }

    }
}
