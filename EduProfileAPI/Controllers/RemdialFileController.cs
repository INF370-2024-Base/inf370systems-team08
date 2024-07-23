using EduProfileAPI.Models;
using EduProfileAPI.Repositories.Interfaces;
using EduProfileAPI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;

namespace EduProfileAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RemedialFileController : ControllerBase
    {
        private readonly IRemedialFileRepository _remFileRepo;

        public RemedialFileController(IRemedialFileRepository remFileRepo)
        {
            _remFileRepo = remFileRepo;
        }

        [HttpGet]
        [Route("GetAllRemedialFiles")]
        public async Task<IActionResult> GetAllRemedialFilesAsync()
        {
            try
            {
                var result = await _remFileRepo.GetAllRemedialFileAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error. Please contact support. {ex.Message}");
            }
        }

        [HttpGet]
        [Route("GetRemedialFiles/{remedialFileId}")]
        public async Task<IActionResult> GetRemedialFilesAsync(Guid remedialFileId)
        {
            try
            {
                var results = await _remFileRepo.GetRemedialFileAsync(remedialFileId);
                if (results == null) return NotFound("Remedial file does not exist");
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
            if (rvm == null || rvm.RemFileId == Guid.Empty)
            {
                return BadRequest("RemedialFileVM cannot be null and RemFileId must be a valid GUID");
            }

            // Create a RemedialFile object from the ViewModel
            var remedialFile = new RemedialFile
            {
                RemFileId = rvm.RemFileId,
                EmployeeId = rvm.EmployeeId,
                SubjectId = rvm.SubjectId,
                Title = rvm.Title,
                Description = rvm.Description,
                Date = rvm.Date
            };

            try
            {
                _remFileRepo.Add(remedialFile);
                await _remFileRepo.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest($"Invalid transaction: {ex.Message}");
            }

            return Ok(remedialFile);
        }

        [HttpPut]
        [Route("EditRemedialFiles/{remedialFileId}")]
        public async Task<ActionResult<RemedialFileVM>> EditRemedialFiles(Guid remedialFileId, RemedialFileVM remFileVM)
        {
            try
            {
                var existingRemedialFile = await _remFileRepo.GetRemedialFileAsync(remedialFileId);

                if (existingRemedialFile == null)
                    return NotFound($"The remedial file does not exist");

                existingRemedialFile.EmployeeId = remFileVM.EmployeeId;
                existingRemedialFile.SubjectId = remFileVM.SubjectId;
                existingRemedialFile.Title = remFileVM.Title;
                existingRemedialFile.Description = remFileVM.Description;
                existingRemedialFile.Date = remFileVM.Date;

                if (await _remFileRepo.SaveChangesAsync())
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
        [Route("DeleteRemedialFiles/{remedialFileId}")]
        public async Task<IActionResult> DeleteRemedialFiles(Guid remedialFileId)
        {
            try
            {
                var existingRemedialFile = await _remFileRepo.GetRemedialFileAsync(remedialFileId);
                if (existingRemedialFile == null) return NotFound($"The remedial file does not exist");
                _remFileRepo.Delete(existingRemedialFile);

                if (await _remFileRepo.SaveChangesAsync()) return Ok(existingRemedialFile);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
            return BadRequest("Your request is invalid");
        }
    }
}
