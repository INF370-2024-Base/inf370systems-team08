using EduProfileAPI.Models;
using EduProfileAPI.Repositories.Interfaces;
using EduProfileAPI.Repositories.Interfaces.Maintenance;
using EduProfileAPI.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EduProfileAPI.Controllers.Maintenance
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaintenancePriorityController : ControllerBase
    {
        private readonly IMaintenancePriority _priorityRepository;

        public MaintenancePriorityController(IMaintenancePriority priorityRepository)
        {
            _priorityRepository = priorityRepository;
        }

        [HttpGet]
        [Route("GetAllPriority")] 
        public async Task<IActionResult> GetAllPriority()
        {
            try
            {
                var results = await _priorityRepository.GetAllPriorityAsync();
                return Ok(results);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }

        //[HttpPost]
        //[Route("AddMerit")]
        //public async Task<IActionResult> AddMerit(CreateMeritVM cvm)
        //{
        //    var merit = new Merit { MeritTypeId = cvm.MeritTypeId, EmployeeId = cvm.EmployeeId, StudentId = cvm.StudentId, MeritName = cvm.MeritName, MeritDescription = cvm.MeritDescription };

        //    try
        //    {
        //        _meritRepository.Add(merit);
        //        await _meritRepository.SaveChangesAsync();
        //    }
        //    catch (Exception)
        //    {
        //        return BadRequest("Invalid transaction");
        //    }

        //    return Ok(merit);
        //}
    }
}
