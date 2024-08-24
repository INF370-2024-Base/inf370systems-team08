using EduProfileAPI.Models;
using EduProfileAPI.Repositories.Interfaces;
using EduProfileAPI.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EduProfileAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentDocumentTypeController : ControllerBase
    {
        private readonly IStudentDocumentType _studentDocumentTypeRepo;

        public StudentDocumentTypeController(IStudentDocumentType studentDocumentTypeRepo)
        {
            _studentDocumentTypeRepo = studentDocumentTypeRepo;
        }

        [HttpGet]
        [Route("GetAllStudentDocumentTypes")] //returns a list of merits
        public async Task<IActionResult> GetAllStudentDocumentTypes()
        {
            try
            {
                var results = await _studentDocumentTypeRepo.GetAllDocTypesAsync();
                return Ok(results);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }

        [HttpGet]
        [Route("GetStudentDocumentType/{studentDocumentTypeId}")] // returns a specific merit
        public async Task<IActionResult> GetStudentDocumentTypeAsync(Guid studentDocumentTypeId)
        {
            try
            {
                var results = await _studentDocumentTypeRepo.GetDocTypeAsync(studentDocumentTypeId);

                if (results == null) return NotFound("Student Document type type does not exist");

                return Ok(results);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }

        [HttpPost]
        [Route("AddStudentDocumentType")]
        public async Task<IActionResult> AddStudentDocumentType(StudentDocumentTypeVM cvm)
        {
            var type = new StudentDocumentType { StudentDocumentTypeName = cvm.StudentDocumentTypeName };

            try
            {
                _studentDocumentTypeRepo.Add(type);
                await _studentDocumentTypeRepo.SaveChangesAsync();
            }
            catch (Exception)
            {
                return BadRequest("Invalid transaction");
            }

            return Ok(type);
        }

        [HttpPut]
        [Route("EditStudentDocumentType/{studentDocumentTypeId}")]
        public async Task<ActionResult<StudentDocumentType>> EditStudentDocumentType(Guid studentDocumentTypeId, StudentDocumentTypeVM model)
        {
            try
            {
                var existing = await _studentDocumentTypeRepo.GetDocTypeAsync(studentDocumentTypeId);
                if (existing == null) return NotFound($"The student document type does not exist");
                existing.StudentDocumentTypeName = model.StudentDocumentTypeName;


                if (await _studentDocumentTypeRepo.SaveChangesAsync())
                {
                    return Ok(existing);
                }

            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");

            }
            return BadRequest("Your request is invalid.");


        }

        [HttpDelete]
        [Route("DeleteStudentDocumentType/{studentDocumentTypeId}")]
        public async Task<IActionResult> DeleteStudentDocumentType(Guid studentDocumentTypeId)
        {
            try
            {
                var existing = await _studentDocumentTypeRepo.GetDocTypeAsync(studentDocumentTypeId);
                if (existing == null) return NotFound($"The merit Type does not exist");
                _studentDocumentTypeRepo.Delete(existing);

                if (await _studentDocumentTypeRepo.SaveChangesAsync()) return Ok(existing);
            }
            catch (Exception)
            {

                return StatusCode(500, "Internal Server Error. Please contact support.");
            }

            return BadRequest("Your request is invalid");
        }
    }
}
