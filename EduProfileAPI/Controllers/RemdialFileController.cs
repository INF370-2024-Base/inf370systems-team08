using EduProfileAPI.Models;
using EduProfileAPI.Repositories.Interfaces;
using EduProfileAPI.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EduProfileAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RemdialFileController : ControllerBase
    {
        private readonly IRemedialFileRepository _RemFileRepo;

        public RemdialFileController(IRemedialFileRepository RemFileRepo)
        {
            _RemFileRepo = RemFileRepo;
        }

        [HttpGet]
        [Route("GetAllRemedialFiles")]
        public async Task<IActionResult> GetAllRemedialFilesAsync()
        {
            try
            {
                var result = await _RemFileRepo.GetAllRemedialFileAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error. Please contact support. {ex.Message}");
            }
        }

        [HttpGet]
        [Route("GetRemedialFiles/{remedialFileId}")]
        public async Task<IActionResult> GetRemedialFilesAsync(Guid remFileId)
        {
            try
            {
                var results = await _RemFileRepo.GetRemedialFileAsync(remFileId);

                if (results == null) return NotFound("Class does not exist");

                return Ok(results);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support");
            }
        }

        [HttpPost]
        [Route("AddRemedialFiles")]
        public async Task<IActionResult> AddRemedialFiles(RemedialFileVM rvm)
        {
            if (rvm == null)
            {
                return BadRequest("RemedialFileVM cannot be null");
            }

            // Create a RemedialFile object from the ViewModel
            var remedialFile = new RemedialFile
            {
                EmployeeId = rvm.EmployeeId,
                SubjectId = rvm.SubjectId,
                Title = rvm.Title,
                Description = rvm.Description,
                Date = rvm.Date
            };

            try
            {
                // Add the RemedialFile object to the repository
                _RemFileRepo.Add(remedialFile);
                await _RemFileRepo.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                return BadRequest("Invalid transaction");
            }

            return Ok(remedialFile);
        }



        [HttpPut]
        [Route("EditRemedialFiles/{RemedialFileId}")]
        public async Task<ActionResult<RemedialFileVM>> EditRemedialFiles(Guid remFileId, RemedialFileVM remFileVM)
        {

            try
            {
                var existingRemedialFile = await _RemFileRepo.GetRemedialFileAsync(remFileId);

                if (existingRemedialFile == null)
                    return NotFound($"The remedial file does not exist");


                existingRemedialFile.EmployeeId = remFileVM.EmployeeId;
                existingRemedialFile.SubjectId = remFileVM.SubjectId;
                existingRemedialFile.Title = remFileVM.Title;
                existingRemedialFile.Description = remFileVM.Description;
                existingRemedialFile.Date = remFileVM.Date;

                if (await _RemFileRepo.SaveChangesAsync())
                {
                    return Ok(existingRemedialFile);
                }
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }

            return BadRequest("Your request is invalid.");

        }

        [HttpDelete]
        [Route("DeleteRemedialFiles/{RemedialFileId}")]
        public async Task<IActionResult> DeleteRemedialFiles(Guid remFileId)
        {

            try
            {
                var existingRemedialFile = await _RemFileRepo.GetRemedialFileAsync(remFileId);
                if (existingRemedialFile == null) return NotFound($"The remedial file does not exist");
                _RemFileRepo.Delete(existingRemedialFile);

                if (await _RemFileRepo.SaveChangesAsync()) return Ok(existingRemedialFile);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
            return BadRequest("Your request is invalid");
        }
    }
}

