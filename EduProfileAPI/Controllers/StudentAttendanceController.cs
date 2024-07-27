using EduProfileAPI.Repositories.Interfaces;
using EduProfileAPI.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EduProfileAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentAttendanceController : ControllerBase
    {
        private readonly IStudentAttendanceRepo _studentAttendanceRepo;
        public StudentAttendanceController(IStudentAttendanceRepo studentAttendanceRepo)
        {
            _studentAttendanceRepo = studentAttendanceRepo;
        }
        // teacher assigned to the class can do attendance


        [HttpGet]
        [Route("GetClassList/{classId}")]
        public async Task<IActionResult> GetClassListByClassAsync(Guid classId)
        {
            try
            {
                var students = await _studentAttendanceRepo.GetStudentClassListByClass(classId);
                if (students == null /*|| !students.Any()*/)
                {
                    return NotFound("No students found for the specified class.");
                }
                return Ok(students);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Database Failure: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("GetAttendanceStatus")]
        public async Task<IActionResult> GetAllAttendanceStatus()
        {
            try
            {
                var status = await _studentAttendanceRepo.GetAllAttendanceStatus();
                return Ok(status);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Database Failure: " + ex.Message });
            }

        }

        [HttpPost]
        [Route("RecordStudentAttendance")]
        public async Task<IActionResult> RecordStudentAttendance([FromBody] StudentAttendanceViewModel model)
        {
            try
            {
                if (model == null)
                {
                    return BadRequest("Invalid Input");
                }

                var result = await _studentAttendanceRepo.RecordStudentAttedance(model);
                if ( result == null)
                {
                    return NotFound("Class, Student or Teahcer not found in the database");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Database Failure: " + ex.Message });
            }
        }

        [HttpPut]
        [Route("UpdateStudentAttendance/{studentId}")]
        public async Task<IActionResult> UpdateStudentAttendance(Guid studentId, [FromBody] UpdateStudentAttendanceVM model)
        {
            try
            {
                if (model == null)
                {
                    return BadRequest("Invalid Input");
                }

                var result = await _studentAttendanceRepo.UpdateStudentAttendance(studentId, model);
                if (result == null)
                {
                    return NotFound("Student not found in the database");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Database Failure: " + ex.Message });
            }
        }

        [HttpGet]
        [Route("GetStudentAttendance/{studentAttendanceId}")]
        public async Task<IActionResult> GetStudentAttendance(Guid studentAttendanceId)
        {
            try
            {
                var result = await _studentAttendanceRepo.GetStudentAttendanceById(studentAttendanceId);
                if (result == null)
                {
                    return NotFound("No attendance records found in the database");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Database Failure: " + ex.Message });
            }
        }
    }
}
