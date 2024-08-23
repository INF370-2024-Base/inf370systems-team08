using EduProfileAPI.Repositories.Implementation;
using EduProfileAPI.Repositories.Interfaces;
using EduProfileAPI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.Pkcs;

namespace EduProfileAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssignStudentController : ControllerBase
    {
        private readonly IAssignStudentRepo _repository;

        public AssignStudentController(IAssignStudentRepo assignRepository)
        {
            _repository = assignRepository;
        }


        [HttpPut("AssignStudentToClass")]
        public async Task<IActionResult> AssignStudentToClass(Guid studentId, Guid classId)
        {
            await _repository.AssignStudentToClassAsync(studentId, classId);
            return Ok();

        }

        [HttpPost("AssignStudentToSubject")]
        public async Task<IActionResult> AssignStudentToSubject([FromBody] StudentSubjectVM request)
        {
            var studentsub  = new StudentSubjectVM { StudentId = request.StudentId, SubjectId = request.SubjectId, GradeId = request.GradeId };

            try
            {
                await _repository.AssignStudentToSubjectAsync(studentsub);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest($"Internal Server Error: {ex.Message}");
            }
        }

        [HttpPut("AssignStudentToGrade")]
        public async Task<IActionResult> AssignStudentToGrade(Guid studentId, Guid gradeId)
        {
            await _repository.AssignStudentToGradeAsync(studentId, gradeId);
            return Ok();
        }

        [HttpGet("GetStudentsByClassId/{classId}")]
        public async Task<IActionResult> GetStudentsByClassId(Guid classId)
        {
            var students = await _repository.GetStudentsByClassIdAsync(classId);
            return Ok(students);
        }

        [HttpGet("GetStudentsByGradeId/{gradeId}")]
        public async Task<IActionResult> GetStudentsByGradeId(Guid gradeId)
        {
            var students = await _repository.GetStudentsByGradeIdAsync(gradeId);
            return Ok(students);
        }

        [HttpGet("GetStudentsBySubjectId/{subjectId}")]
        public async Task<IActionResult> GetStudentsBySubjectId(Guid subjectId)
        {
            var students = await _repository.GetStudentsBySubjectIdAsync(subjectId);
            return Ok(students);
        }

        [HttpGet("GetEverything")]
        public async Task<IActionResult> GetFilteredStudents([FromQuery] string classId, [FromQuery] string gradeId, [FromQuery] string subjectId)
        {
            Guid? classGuid = !string.IsNullOrEmpty(classId) ? Guid.Parse(classId) : null;
            Guid? gradeGuid = !string.IsNullOrEmpty(gradeId) ? Guid.Parse(gradeId) : null;
            Guid? subjectGuid = !string.IsNullOrEmpty(subjectId) ? Guid.Parse(subjectId) : null;

            var students = await _repository.GetFilteredStudentsAsync(classGuid, gradeGuid, subjectGuid);
            return Ok(students);
        }
    }
}
