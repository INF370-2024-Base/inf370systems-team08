using EduProfileAPI.Repositories.Implementation;
using Microsoft.AspNetCore.Mvc;

namespace EduProfileAPI.Controllers
{
    public class AssignStudentController : Controller
    {
        private AssignStudentRepo _repository;

        public AssignStudentController(AssignStudentRepo repository)
        {
            _repository = repository;
        }

        [HttpGet("GetAllStudents")]
        public async Task<IActionResult> GetAllStudents()
        {
            var students = await _repository.GetAllStudentsAsync();
            return Ok(students);
        }

        [HttpGet("GetAllClasses")]
        public async Task<IActionResult> GetAllClasses()
        {
            var classes = await _repository.GetAllClassesAsync();
            return Ok(classes);
        }

        [HttpGet("GetAllGrades")]
        public async Task<IActionResult> GetAllGrades()
        {
            var grades = await _repository.GetAllGradesAsync();
            return Ok(grades);
        }

        [HttpGet("GetAllSubjects")]
        public async Task<IActionResult> GetAllSubjects()
        {
            var subjects = await _repository.GetAllSubjectAsync();
            return Ok(subjects);
        }

        [HttpPut("AssignStudentToClass")]
        public async Task<IActionResult> AssignStudentToClass(Guid studentId, Guid classId, Guid gradeId)
        {
            await _repository.AssignStudentToClassAsync(studentId, classId, gradeId);
            return Ok();

        }

        //[HttpPut("AssignStudentToSubject")]
        //public async Task<IActionResult> AssignStudentToSubject(Guid studentId, Guid subjectId)
        //{
        //    await _repository.AssignStudentToSubjectAsync(studentId, subjectId);
        //    return Ok();
        //}

        [HttpPut("AssignStudentToGrade")]
        public async Task<IActionResult> AssignStudentToGrade(Guid studentId, Guid gradeId)
        {
            await _repository.AssignStudentToGradeAsync(studentId, gradeId);
            return Ok();
        }
    }
}
