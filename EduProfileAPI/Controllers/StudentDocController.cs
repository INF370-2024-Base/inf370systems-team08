using EduProfileAPI.Models;
using EduProfileAPI.Repositories.Interfaces;
using EduProfileAPI.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EduProfileAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentDocController : ControllerBase
    {
        private readonly IStudentDocRepository _studentDocRepository;

        public StudentDocController(IStudentDocRepository studentDocRepository)
        {
            _studentDocRepository = studentDocRepository;
        }

        [HttpGet]
        [Route("GetAllStudentDocs")] 
        public async Task<IActionResult> GetAllStudentDocs()
        {
            try
            {
                var results = await _studentDocRepository.GetAllStudentDocsAsync();
                return Ok(results);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error. Please contact support. Error details: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("GetStudentDocs/{studentDocId}")] // returns a specific merit
        public async Task<IActionResult> GetStudentDocAsync(Guid studentDocId)
        {
            try
            {
                var results = await _studentDocRepository.GetStudentDocAsync(studentDocId);

                if (results == null) return NotFound("Student document does not exist");

                return Ok(results);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }

        [HttpPost]
        [Route("AddStudentDoc")]
        public async Task<IActionResult> AddStudentDoc([FromForm] StudentDocVM cvm)
        {
            if (cvm.StudentDocumentAttachment == null || cvm.StudentDocumentAttachment.Length == 0)
                return BadRequest("No file uploaded");

            byte[] fileContent;
            using (var memoryStream = new MemoryStream())
            {
                await cvm.StudentDocumentAttachment.CopyToAsync(memoryStream);
                fileContent = memoryStream.ToArray();
            }

            var studoc = new StudentDoc
            {
                StudentId = cvm.StudentId,
                DocumentTypeId = cvm.DocumentTypeId,
                DocumentName = cvm.DocumentName ?? cvm.StudentDocumentAttachment.FileName,  // Use uploaded file name if no name provided
                StudentDocumentAttachment = fileContent,
                AttachmentType = cvm.StudentDocumentAttachment.ContentType
    };

            try
            {
                _studentDocRepository.Add(studoc);
                await _studentDocRepository.SaveChangesAsync();
                return Ok(studoc);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it accordingly
                return BadRequest("Invalid transaction");
            }
           
        }

        [HttpPut]
        [Route("EditStudentDoc/{studentDocId}")]
        public async Task<ActionResult<StudentDocVM>> EditStudentDoc(Guid studentDocId, [FromForm] StudentDocVM studentDocModel)
        {
            try
            {
                // Check if the document exists
                var existingstudoc = await _studentDocRepository.GetStudentDocAsync(studentDocId);
                if (existingstudoc == null)
                    return NotFound($"The student document with ID {studentDocId} does not exist.");

                // Validate the required fields
                if (studentDocModel.StudentId == Guid.Empty)
                    return BadRequest("Student ID is required.");

                if (studentDocModel.DocumentTypeId == Guid.Empty)
                    return BadRequest("Document Type ID is required.");

                if (string.IsNullOrWhiteSpace(studentDocModel.DocumentName) && studentDocModel.StudentDocumentAttachment == null)
                    return BadRequest("Either Document Name or an attachment must be provided.");

                // Validate the file type and size if an attachment is provided
                if (studentDocModel.StudentDocumentAttachment != null)
                {
                    var allowedFileTypes = new[] { "application/pdf", "image/jpeg", "image/png" };
                    if (!allowedFileTypes.Contains(studentDocModel.StudentDocumentAttachment.ContentType))
                        return BadRequest($"Unsupported file type. Allowed types are: {string.Join(", ", allowedFileTypes)}");

                    if (studentDocModel.StudentDocumentAttachment.Length > 10 * 1024 * 1024) // 10MB limit
                        return BadRequest("File size must be less than 10MB.");
                }

                //var existingstudoc = await _studentDocRepository.GetStudentDocAsync(studentDocId);
                //if (existingstudoc == null) return NotFound($"The student document does not exist");

                existingstudoc.StudentId = studentDocModel.StudentId;
                existingstudoc.DocumentTypeId = studentDocModel.DocumentTypeId;
                existingstudoc.DocumentName = studentDocModel.DocumentName ?? studentDocModel.StudentDocumentAttachment?.FileName;
                existingstudoc.AttachmentType = studentDocModel.StudentDocumentAttachment.ContentType;

                if (studentDocModel.StudentDocumentAttachment != null && studentDocModel.StudentDocumentAttachment.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await studentDocModel.StudentDocumentAttachment.CopyToAsync(memoryStream);
                        existingstudoc.StudentDocumentAttachment = memoryStream.ToArray();
                    }
                }

                if (await _studentDocRepository.SaveChangesAsync())
                {
                    return Ok(existingstudoc);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error. Please contact support. {ex}");
            }
            return BadRequest("Your request is invalid.");
        


    }

        [HttpDelete]
        [Route("DeleteStudentDoc/{studentDocId}")]
        public async Task<IActionResult> DeleteStudentDoc(Guid studentDocId)
        {
            try
            {
                var existingstudoc = await _studentDocRepository.GetStudentDocAsync(studentDocId);
                if (existingstudoc == null) return NotFound($"The Student document does not exist");
                _studentDocRepository.Delete(existingstudoc);

                if (await _studentDocRepository.SaveChangesAsync()) return Ok(existingstudoc);
            }
            catch (Exception)
            {

                return StatusCode(500, "Internal Server Error. Please contact support.");
            }

            return BadRequest("Your request is invalid");
        }

        [HttpGet]
        [Route("DownloadAttachment/{stuDocumentId}")]
        public async Task<IActionResult> DownloadAttachment(Guid stuDocumentId)
        {
            try
            {
                var doc = await _studentDocRepository.GetStudentDocAsync(stuDocumentId);
                if (doc == null || doc.StudentDocumentAttachment == null)
                {
                    return NotFound("Attachment not found.");
                }

                var fileContent = doc.StudentDocumentAttachment;
                var fileName = doc.DocumentName;
                var contentType = doc.AttachmentType; 

                return File(fileContent, contentType, fileName);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error. Please contact support. {ex.Message}");
            }
        }
    }
}

