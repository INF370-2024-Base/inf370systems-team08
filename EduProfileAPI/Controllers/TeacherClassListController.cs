using EduProfileAPI.Repositories.Interfaces;
using EduProfileAPI.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EduProfileAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherClassListController : ControllerBase
    {
        private readonly ITeacherClassListRepo _teacherClassListRepo;
        public TeacherClassListController(ITeacherClassListRepo teacherClassListRepo)
        {
            _teacherClassListRepo = teacherClassListRepo;
        }

        [HttpGet("GetStudentsInClass/{classId}")]
        public async Task<IActionResult> GetStudentsInClass(Guid classId)
        {
            try
            {
                var results = await _teacherClassListRepo.StudentsInClass(classId);
                return Ok(results);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error. Please contact support. Error details: {ex.Message}");
            }
        }

    }
}
