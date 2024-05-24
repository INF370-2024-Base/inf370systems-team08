using EduProfileAPI.Repositories.Interfaces;
using EduProfileAPI.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EduProfileAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssignTeacherToClassController : ControllerBase
    {
        private readonly IAssignTeacherToClassRepository _assignTeacherToClassRepository;

        public AssignTeacherToClassController(IAssignTeacherToClassRepository assignTeacherToClassRepository)
        {
            _assignTeacherToClassRepository = assignTeacherToClassRepository;
        }

        [HttpPut]
        [Route("AssignTeacherToClass")]
        public async Task<IActionResult> AssignTeacherToClassAsync([FromBody] AssignTeacherToClassViewModel model)
        {
            try
            {
                if (model == null)
                { 
                    return BadRequest("Invalid data");
                }

                var assingResult = await _assignTeacherToClassRepository.AssignTeacherToClassAsync(model);
                if (assingResult == null)
                {
                    return NotFound("Class or Teacher not found");
                }

                return Ok(assingResult);

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error. Please contact support. {ex.Message}");
            }
        
        }
            
            
    }
}
