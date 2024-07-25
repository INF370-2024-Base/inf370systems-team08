using EduProfileAPI.Models;
using EduProfileAPI.Repositories.Interfaces;
using EduProfileAPI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace EduProfileAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RemedialFileController : ControllerBase
    {
        private readonly IRemedialFileRepository _remFileRepo;
        private readonly IRemedialActivityRepository _remActRepo;

        public RemedialFileController(IRemedialFileRepository remFileRepo, IRemedialActivityRepository remActRepo)
        {
            _remFileRepo = remFileRepo;
            _remActRepo = remActRepo;
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
        [Route("GetRemedialFile/{remedialFileId}")]
        public async Task<IActionResult> GetRemedialFileAsync(Guid remedialFileId)
        {
            try
            {
                var result = await _remFileRepo.GetRemedialFileAsync(remedialFileId);
                if (result == null) return NotFound("Remedial file does not exist");
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }

        [HttpPost]
        [Route("AddRemedialFile")]
        public async Task<IActionResult> AddRemedialFile(RemedialFileVM rvm)
        {
            if (rvm == null)
            {
                return BadRequest("RemedialFileVM cannot be null");
            }

            // Create a new GUID for the RemedialFile
            var remedialFileId = Guid.NewGuid();

            // Create a RemedialFile object from the ViewModel
            var remedialFile = new RemedialFile
            {
                RemFileId = remedialFileId,
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
                return Ok(remedialFile);
            }
            catch (Exception ex)
            {
                return BadRequest($"Invalid transaction: {ex.Message}");
            }
        }

        [HttpPut]
        [Route("EditRemedialFile/{remedialFileId}")]
        public async Task<IActionResult> EditRemedialFile(Guid remedialFileId, RemedialFileVM remFileVM)
        {
            try
            {
                var existingRemedialFile = await _remFileRepo.GetRemedialFileAsync(remedialFileId);

                if (existingRemedialFile == null)
                    return NotFound("The remedial file does not exist");

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
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error. Please contact support. {ex.Message}");
            }

            return BadRequest("Your request is invalid.");
        }

        [HttpDelete]
        [Route("DeleteRemedialFile/{remedialFileId}")]
        public async Task<IActionResult> DeleteRemedialFile(Guid remedialFileId)
        {
            try
            {
                var existingRemedialFile = await _remFileRepo.GetRemedialFileAsync(remedialFileId);
                if (existingRemedialFile == null) return NotFound("The remedial file does not exist");
                _remFileRepo.Delete(existingRemedialFile);

                if (await _remFileRepo.SaveChangesAsync()) return Ok(existingRemedialFile);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error. Please contact support. {ex.Message}");
            }
            return BadRequest("Your request is invalid");
        }

        [HttpGet]
        [Route("DownloadAttachment/{remActId}")]
        public async Task<IActionResult> DownloadAttachment(Guid remActId)
        {
            try
            {
                var activity = await _remActRepo.GetRemedialActivityAsync(remActId);
                if (activity == null || activity.Attachment == null)
                {
                    return NotFound("Attachment not found");
                }

                var fileContent = activity.Attachment;
                var fileName = activity.Title.EndsWith(".pdf") ? activity.Title : activity.Title + ".pdf";

                return File(fileContent, "application/pdf", fileName);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error. Please contact support. {ex.Message}");
            }
        }
    }
}
