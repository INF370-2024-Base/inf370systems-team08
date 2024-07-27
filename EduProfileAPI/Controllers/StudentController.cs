using EduProfileAPI.Models;
using EduProfileAPI.Repositories.Interfaces;
using EduProfileAPI.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EduProfileAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentRepository _studentRepository;

        public StudentController(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        [HttpGet]
        [Route("GetAllStudents")] 
        public async Task<IActionResult> GetAllStudents()
        {
            try
            {
                var results = await _studentRepository.GetAllStudentsAsync();
                return Ok(results);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error. Please contact support. Error details: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("GetStudent/{studentId}")] // returns a specific merit
        public async Task<IActionResult> GetStudentAsync(Guid studentId)
        {
            try
            {
                var results = await _studentRepository.GetStudentAsync(studentId);

                if (results == null) return NotFound("Student does not exist");

                return Ok(results);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }

        [HttpPost]
        [Route("AddStudent")]
        public async Task<IActionResult> AddStudent(StudentVM cvm)
        {
            var student = new Student { GradeId = cvm.GradeId, ClassId = cvm.ClassId, ParentId = cvm.ParentId, FirstName = cvm.FirstName, LastName = cvm.LastName, DateOfBirth = cvm.DateOfBirth, Gender = cvm.Gender, Address = cvm.Address, AdmissionNo = cvm.AdmissionNo, EmergencyContactName = cvm.EmergencyContactName, EmergencyContactRelationship = cvm.EmergencyContactRelationship, EmergencyContactPhoneNum = cvm.EmergencyContactPhoneNum };

            try
            {
                _studentRepository.Add(student);
                await _studentRepository.SaveChangesAsync();
            }
            catch (Exception)
            {
                return BadRequest("Invalid transaction");
            }

            return Ok(student);
        }

        [HttpPut]
        [Route("EditStudent/{studentId}")]
        public async Task<ActionResult<StudentVM>> EditStudent(Guid studentId, StudentVM studentModel)
        {
            try
            {
                var existingStudent = await _studentRepository.GetStudentAsync(studentId);
                if (existingStudent == null) return NotFound($"The Student does not exist");
                existingStudent.GradeId = studentModel.GradeId;
                existingStudent.ClassId = studentModel.ClassId;
                existingStudent.ParentId = studentModel.ParentId;
                existingStudent.FirstName = studentModel.FirstName;
                existingStudent.LastName = studentModel.LastName;
                existingStudent.DateOfBirth = studentModel.DateOfBirth;
                existingStudent.Gender = studentModel.Gender;
                existingStudent.Address = studentModel.Address;
                existingStudent.AdmissionNo = studentModel.AdmissionNo;
                existingStudent.EmergencyContactName = studentModel.EmergencyContactName;
                existingStudent.EmergencyContactRelationship = studentModel.EmergencyContactRelationship;
                existingStudent.EmergencyContactPhoneNum = studentModel.EmergencyContactPhoneNum;
          

                if (await _studentRepository.SaveChangesAsync())
                {
                    return Ok(existingStudent);
                }

            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");

            }
            return BadRequest("Your request is invalid.");


        }

        [HttpDelete]
        [Route("DeleteStudent/{studentId}")]
        public async Task<IActionResult> DeleteStudent(Guid studentId)
        {
            try
            {
                var existingStudent = await _studentRepository.GetStudentAsync(studentId);
                if (existingStudent == null) return NotFound($"The merit does not exist");
                _studentRepository.Delete(existingStudent);

                if (await _studentRepository.SaveChangesAsync()) return Ok(existingStudent);
            }
            catch (Exception)
            {

                return StatusCode(500, "Internal Server Error. Please contact support.");
            }

            return BadRequest("Your request is invalid");
        }

        [HttpGet]
        [Route("GetParents")]
        public async Task<ActionResult<Parent[]>> GetAllParents()
        {
            var parents = await _studentRepository.GetAllParentsAsync();
            return Ok(parents);
        }
    }
}
