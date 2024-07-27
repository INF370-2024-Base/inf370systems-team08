using EduProfileAPI.Models;
using EduProfileAPI.Repositories.Interfaces;
using EduProfileAPI.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace EduProfileAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RemedialActivityController : ControllerBase
    {
        private readonly IRemedialActivityRepository _remedialActivityRepository;

        public RemedialActivityController(IRemedialActivityRepository remedialActivityRepository)
        {
            _remedialActivityRepository = remedialActivityRepository;
        }

        [HttpGet]
        [Route("GetAllRemedialActivities")]
        public async Task<IActionResult> GetAllRemedialActivities()
        {
            try
            {
                var results = await _remedialActivityRepository.GetAllRemedialActivitiesAsync();
                return Ok(results);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error. Please contact support. Error details: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("GetRemedialActivity/{remedialActivityId}")]
        public async Task<IActionResult> GetRemedialActivity(Guid remedialActivityId)
        {
            try
            {
                var results = await _remedialActivityRepository.GetRemedialActivityAsync(remedialActivityId);

                if (results == null) return NotFound("Remedial Activity does not exist");

                return Ok(results);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }

        [HttpPost]
        [Route("AddRemedialActivity")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> AddRemedialActivity([FromForm] RemedialActivityVM model)
        {
            if (model.Attachment == null || model.Attachment.Length == 0)
                return BadRequest("No file uploaded");

            byte[] fileContent;
            using (var memoryStream = new MemoryStream())
            {
                await model.Attachment.CopyToAsync(memoryStream);
                fileContent = memoryStream.ToArray();
            }

            var remAct = new RemedialActivity
            {
                RemFileId = model.RemFileId,
                Title = model.Title ?? model.Attachment.FileName,  // Use uploaded file name if no name provided
                Description = model.Description,
                Date = model.Date,
                Attachment = fileContent,
                AttachmentName = model.Attachment.FileName,
                AttachmentType = model.Attachment.ContentType
            };

            try
            {
                _remedialActivityRepository.Add(remAct);
                await _remedialActivityRepository.SaveChangesAsync();
                return Ok(remAct);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it accordingly
                return BadRequest("Invalid transaction");
            }
        }

        [HttpPut]
        [Route("EditRemedialActivity/{remedialActivityId}")]
        public async Task<IActionResult> EditRemedialActivity(Guid remedialActivityId, [FromForm] RemedialActivityVM model)
        {
            try
            {
                var existingRemAct = await _remedialActivityRepository.GetRemedialActivityAsync(remedialActivityId);
                if (existingRemAct == null) return NotFound($"The remedial activity does not exist");

                existingRemAct.RemFileId = model.RemFileId;
                existingRemAct.Title = model.Title ?? model.Attachment?.FileName;
                existingRemAct.Description = model.Description;
                existingRemAct.Date = model.Date;

                if (model.Attachment != null && model.Attachment.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await model.Attachment.CopyToAsync(memoryStream);
                        existingRemAct.Attachment = memoryStream.ToArray();
                        existingRemAct.AttachmentName = model.Attachment.FileName;
                        existingRemAct.AttachmentType = model.Attachment.ContentType;
                    }
                }

                if (await _remedialActivityRepository.SaveChangesAsync())
                {
                    return Ok(existingRemAct);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error. Please contact support.{ex}");
            }
            return BadRequest("Your request is invalid.");
        }

        [HttpDelete]
        [Route("DeleteRemedialActivity/{remedialActivityId}")]
        public async Task<IActionResult> DeleteRemedialActivity(Guid remedialActivityId)
        {
            try
            {
                var existingRemAct = await _remedialActivityRepository.GetRemedialActivityAsync(remedialActivityId);
                if (existingRemAct == null) return NotFound($"The remedial activity does not exist");
                _remedialActivityRepository.Delete(existingRemAct);

                if (await _remedialActivityRepository.SaveChangesAsync()) return Ok(existingRemAct);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }

            return BadRequest("Your request is invalid");
        }

        [HttpGet]
        [Route("GetActivitiesByFileId/{remFileId}")]
        public async Task<IActionResult> GetActivitiesByFileId(Guid remFileId)
        {
            try
            {
                var activities = await _remedialActivityRepository.GetActivitiesByFileId(remFileId);
                if (activities == null || activities.Count == 0)
                {
                    return NotFound("No activities found for the provided file ID.");
                }
                return Ok(activities);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("DownloadAttachment/{remActId}")]
        public async Task<IActionResult> DownloadAttachment(Guid remActId)
        {
            try
            {
                var activity = await _remedialActivityRepository.GetRemedialActivityAsync(remActId);
                if (activity == null || activity.Attachment == null)
                {
                    return NotFound("Attachment not found");
                }

                var fileContent = activity.Attachment;
                var fileName = activity.Title;
                var contentType = activity.AttachmentType;

                return File(fileContent, contentType, fileName);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error. Please contact support. {ex.Message}");
            }
        }
    }
}
