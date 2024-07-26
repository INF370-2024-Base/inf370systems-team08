using EduProfileAPI.Models;
using EduProfileAPI.Repositories.Interfaces;
using EduProfileAPI.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EduProfileAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportTypeController : ControllerBase
    {
        private readonly IReportType _reportTypeRepository;

        public ReportTypeController(IReportType reportTypeRepository)
        {
            _reportTypeRepository = reportTypeRepository;
        }

        [HttpGet]
        [Route("GetAllReportTypes")] //returns a list of merits
        public async Task<IActionResult> GetAllReportTypes()
        {
            try
            {
                var results = await _reportTypeRepository.GetAllReportTypesAsync();
                return Ok(results);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }

        [HttpGet]
        [Route("GetReportType/{reportTypeId}")] // returns a specific merit
        public async Task<IActionResult> GetReportTypeAsync(Guid reportTypeId)
        {
            try
            {
                var results = await _reportTypeRepository.GetReportTypeAsync(reportTypeId);

                if (results == null) return NotFound("Report type does not exist");

                return Ok(results);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }

        [HttpPost]
        [Route("AddReportType")]
        public async Task<IActionResult> AddReportType(ReportTypeVM cvm)
        {
            var type = new ReportType { ReportTypeName = cvm.ReportTypeName};

            try
            {
                _reportTypeRepository.Add(type);
                await _reportTypeRepository.SaveChangesAsync();
            }
            catch (Exception)
            {
                return BadRequest("Invalid transaction");
            }

            return Ok(type);
        }


      
    }
}
