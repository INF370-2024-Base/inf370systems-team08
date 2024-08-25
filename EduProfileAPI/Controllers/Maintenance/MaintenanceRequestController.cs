using EduProfileAPI.Models;
using EduProfileAPI.Models.Maintenance;
using EduProfileAPI.Repositories.Interfaces;
using EduProfileAPI.Repositories.Interfaces.Maintenance;
using EduProfileAPI.ViewModels;
using EduProfileAPI.ViewModels.Maintenance;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EduProfileAPI.Controllers.Maintenance
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaintenanceRequestController : ControllerBase
    {
        private readonly IMaintenanceRequest _reqRepository;

        public MaintenanceRequestController(IMaintenanceRequest reqRepository)
        {
            _reqRepository = reqRepository;
        }

        [HttpGet]
        [Route("GetAllRequests")] //returns a list of merits
        public async Task<IActionResult> GetAllRequests()
        {
            try
            {
                var results = await _reqRepository.GetAllRequestsAsync();
                return Ok(results);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("GetRequest/{maintenanceReqId}")] 
        public async Task<IActionResult> GetRequestAsync(Guid maintenanceReqId)
        {
            try
            {
                var results = await _reqRepository.GetRequestAsync(maintenanceReqId);

                if (results == null) return NotFound("Merit does not exist");

                return Ok(results);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }

        [HttpPost]
        [Route("AddRequest")]
        public async Task<IActionResult> AddRequest(MaintenanceRequestVM cvm)
        {
            var request = new MaintenanceRequest { MaintenanceProId = cvm.MaintenanceProId, MaintenanceStatusId = cvm.MaintenanceStatusId, MaintenanceTypeId = cvm.MaintenanceTypeId, EmployeeId = cvm.EmployeeId, RequestDate = cvm.RequestDate, Description = cvm.Description, Location = cvm.Location, AssignedTo = cvm.AssignedTo, ScheduleDate = cvm.ScheduleDate, PriorityId = cvm.PriorityId, Verified = cvm.Verified };

            try
            {
                _reqRepository.Add(request);
                await _reqRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest($"Internal Server Error: {ex.Message}");
            }

            return Ok(request);
        }

        [HttpPut]
        [Route("EditRequest/{maintenanceReqId}")]
        public async Task<ActionResult<MaintenanceRequestVM>> EditRequest(Guid maintenanceReqId, MaintenanceRequestVM mvm)
        {
            try
            {
                var existingReq = await _reqRepository.GetRequestAsync(maintenanceReqId);
                if (existingReq == null) return NotFound($"The merit does not exist");
                existingReq.EmployeeId = mvm.EmployeeId;
                existingReq.MaintenanceProId = mvm.MaintenanceProId;
                existingReq.MaintenanceStatusId = mvm.MaintenanceStatusId;
                existingReq.MaintenanceTypeId = mvm.MaintenanceTypeId;
                existingReq.PriorityId = mvm.PriorityId;
                existingReq.RequestDate = mvm.RequestDate;
                existingReq.Description = mvm.Description;
                existingReq.Location = mvm.Location;
                existingReq.AssignedTo = mvm.AssignedTo;
                existingReq.ScheduleDate = mvm.ScheduleDate;
                existingReq.Verified = mvm.Verified;


                if (await _reqRepository.SaveChangesAsync())
                {
                    return Ok(existingReq);
                }

            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");

            }
            return BadRequest("Your request is invalid.");


        }

        [HttpDelete]
        [Route("DeleteRequest/{maintenanceReqId}")]
        public async Task<IActionResult> DeleteRequest(Guid maintenanceReqId)
        {
            try
            {
                var existingReq = await _reqRepository.GetRequestAsync(maintenanceReqId);
                if (existingReq == null) return NotFound($"The request does not exist");
                _reqRepository.Delete(existingReq);

                if (await _reqRepository.SaveChangesAsync()) return Ok(existingReq);
            }
            catch (Exception)
            {

                return StatusCode(500, "Internal Server Error. Please contact support.");
            }

            return BadRequest("Your request is invalid");
        }
    }
}
