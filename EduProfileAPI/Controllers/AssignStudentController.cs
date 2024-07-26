using EduProfileAPI.Models;
using EduProfileAPI.Repositories.Implementation;
using EduProfileAPI.Repositories.Interfaces;
using EduProfileAPI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SendGrid.Helpers.Mail;

namespace EduProfileAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssignStudentController : Controller
    {
        private IAssignStudentRepo _repository;

        public AssignStudentController(IAssignStudentRepo repository)
        {
            _repository = repository;
        }

        [HttpGet("GetAllStudents")]
        public async Task<IActionResult> GetAllStudents()
        {
            var students = await _repository.GettAllStudentsAsync();
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
        public async Task<IActionResult> AssignStudentToClass(Guid studentId, Guid classId)
        {
            await _repository.AssignStudentToClassAsync(studentId, classId);
            return Ok();

        }

        // POST: /StudentSubjects
        [HttpPost("AssignStudentSubject")]
        public async Task<IActionResult> AddStudentSubject(Guid studentId, Guid subjectId)
        {
            try
            {
                await ((AssignStudentRepo)_repository).AddStudentSubjectAsync(studentId, subjectId);
                return Ok("Student subject relationship added successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred: " + ex.Message);
            }
        }


        [HttpPut("AssignStudentToGrade")]
        public async Task<IActionResult> AssignStudentToGrade(Guid studentId, Guid gradeId)
        {
            await _repository.AssignStudentToGradeAsync(studentId, gradeId);
            return Ok();
        }
    }
}
