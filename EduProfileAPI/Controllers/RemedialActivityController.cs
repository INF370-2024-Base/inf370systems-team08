using EduProfileAPI.Models;
using EduProfileAPI.Repositories.Interfaces;
using EduProfileAPI.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EduProfileAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RemedialActivityController : ControllerBase
    {
        private readonly IRemedialActivityRepository _RemedialActivityRepo;

        public RemedialActivityController(IRemedialActivityRepository RemedialActivityRepo)
        {
            _RemedialActivityRepo = RemedialActivityRepo;
        }

        [HttpGet]
        [Route("GetAllRemedialActivity")]
        public async Task<IActionResult> GetAllRemedialActivity()
        {
            try
            {
                var result = await _RemedialActivityRepo.GetAllRemedialActivitiesAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error. Please contact support. {ex.Message}");
            }
        }

        [HttpGet]
        [Route("GetRemedialActivityById/{remedialActivityId}")]
        public async Task<IActionResult> GetRemedialActivityById(Guid remedialActivityId)
        {
            try
            {
                var results = await _RemedialActivityRepo.GetRemedialActivityByIdAsync(remedialActivityId);

                if (results == null) return NotFound("Remedial activity does not exist");

                return Ok(results);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support");
            }
        }

        [HttpPost]
        [Route("AddRemedialActivity")]
        public async Task<IActionResult> AddRemedialActivity([FromBody] RemedialActivityVM ravm)
        {
            var remedialActivities = new RemedialActivity
            {
                RemActId = ravm.RemActId,
                RemFileId = ravm.RemFileId, // Ensure RemFileId is assigned
                Title = ravm.Title,
                Description = ravm.Description,
                Date = ravm.Date,
                ActivityContent = ravm.ActivityContent
            };

            try
            {
                _RemedialActivityRepo.Add(remedialActivities);
                await _RemedialActivityRepo.SaveChangesAsync();
                return Ok(remedialActivities);
            }
            catch (Exception ex)
            {
                // Log the exception details
                var errorMessage = $"Invalid transaction. {ex.Message}";
                if (ex.InnerException != null)
                {
                    errorMessage += $" Inner Exception: {ex.InnerException.Message}";
                }
                return BadRequest(errorMessage);
            }
        }

        [HttpPut]
        [Route("EditRemedialActivity/{remedialActivityId}")]
        public async Task<ActionResult<RemedialActivityVM>> EditRemedialActivity(Guid remedialActivityId, [FromBody] RemedialActivityVM remActVM)
        {
            try
            {
                var existingRemAct = await _RemedialActivityRepo.GetRemedialActivityByIdAsync(remedialActivityId);

                if (existingRemAct == null)
                    return NotFound("The remedial activity does not exist");

                existingRemAct.RemFileId = remActVM.RemFileId;
                existingRemAct.Title = remActVM.Title;
                existingRemAct.Description = remActVM.Description;
                existingRemAct.Date = remActVM.Date;
                existingRemAct.ActivityContent = remActVM.ActivityContent;

                if (await _RemedialActivityRepo.SaveChangesAsync())
                {
                    return Ok(existingRemAct);
                }
            }
            catch (Exception ex)
            {
                // Log the exception details
                var errorMessage = $"Internal Server Error. Please contact support. {ex.Message}";
                if (ex.InnerException != null)
                {
                    errorMessage += $" Inner Exception: {ex.InnerException.Message}";
                }
                return StatusCode(500, errorMessage);
            }

            return BadRequest("Your request is invalid.");
        }


        [HttpDelete]
        [Route("DeleteRemedialActivity/{remedialActivityId}")]
        public async Task<IActionResult> DeleteRemedialActivity(Guid remedialActivityId)
        {
            try
            {
                var existingRemAct = await _RemedialActivityRepo.GetRemedialActivityByIdAsync(remedialActivityId);
                if (existingRemAct == null) return NotFound($"The remedial activity does not exist");

                _RemedialActivityRepo.Delete(existingRemAct);

                if (await _RemedialActivityRepo.SaveChangesAsync())
                {
                    return Ok(existingRemAct);
                }
            }
            catch (Exception ex)
            {
                // Log the exception here
                return StatusCode(500, $"Internal Server Error. Please contact support. {ex.Message}");
            }
            return BadRequest("Your request is invalid");
        }
    }
}
