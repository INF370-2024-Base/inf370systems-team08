using EduProfileAPI.Models;
using EduProfileAPI.Repositories.Implementation;
using EduProfileAPI.Repositories.Interfaces;
using EduProfileAPI.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EduProfileAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReport _reportRepository;

        public ReportController(IReport reportRepository)
        {
            _reportRepository = reportRepository;
        }

        [HttpGet]
        [Route("GetAllReports")] //returns a list of merits
        public async Task<IActionResult> GetAllReports()
        {
            try
            {
                var results = await _reportRepository.GetAllReportsAsync();
                return Ok(results);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }

        [HttpGet]
        [Route("GetReport/{reportId}")] // returns a specific merit
        public async Task<IActionResult> GetReportAsync(Guid reportId)
        {
            try
            {
                var results = await _reportRepository.GetReportAsync(reportId);

                if (results == null) return NotFound("Report does not exist");

                return Ok(results);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }

        [HttpPost]
        [Route("AddReport")]
        public async Task<IActionResult> AddReport(ReportVM cvm)
        {
            if (cvm.ReportAttachment == null || cvm.ReportAttachment.Length == 0)
                return BadRequest("No file uploaded");

            byte[] fileContent;
            using (var memoryStream = new MemoryStream())
            {
                await cvm.ReportAttachment.CopyToAsync(memoryStream);
                fileContent = memoryStream.ToArray();
            }

            var type = new Report
            {
                ReportTypeId = cvm.ReportTypeId,
                UserId = cvm.UserId,
                Description = cvm.Description,
                CreatedDate = cvm.CreatedDate,
                Title = cvm.Title ?? cvm.ReportAttachment.FileName,  // Use uploaded file name if no name provided
                ReportAttachment = fileContent
            };

            try
            {
                _reportRepository.Add(type);
                await _reportRepository.SaveChangesAsync();
                return Ok(type);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it accordingly
                return BadRequest("Invalid transaction");
            }
        }

        [HttpPut]
        [Route("EditReport/{reportId}")]
        public async Task<ActionResult<ReportVM>> EditReport(Guid reportId, [FromForm] ReportVM model)
        {
            try
            {
                var existingType = await _reportRepository.GetReportAsync(reportId);
                if (existingType == null) return NotFound($"The student document does not exist");

                existingType.ReportTypeId = model.ReportTypeId;
                existingType.Description = model.Description;
                existingType.CreatedDate = model.CreatedDate;
                existingType.Title = model.Title ?? model.ReportAttachment?.FileName;

                if (model.ReportAttachment != null && model.ReportAttachment.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await model.ReportAttachment.CopyToAsync(memoryStream);
                        existingType.ReportAttachment = memoryStream.ToArray();
                    }
                }

                if (await _reportRepository.SaveChangesAsync())
                {
                    return Ok(existingType);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
            return BadRequest("Your request is invalid.");



        }

        [HttpDelete]
        [Route("DeleteReport/{reportId}")]
        public async Task<IActionResult> DeleteReport(Guid reportId)
        {
            try
            {
                var existingType = await _reportRepository.GetReportAsync(reportId);
                if (existingType == null) return NotFound($"The Student document does not exist");
                _reportRepository.Delete(existingType);

                if (await _reportRepository.SaveChangesAsync()) return Ok(existingType);
            }
            catch (Exception)
            {

                return StatusCode(500, "Internal Server Error. Please contact support.");
            }

            return BadRequest("Your request is invalid");
        }

    }
}
