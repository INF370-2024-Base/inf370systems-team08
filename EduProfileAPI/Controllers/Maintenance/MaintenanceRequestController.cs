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
        public async Task<IActionResult> AddRequest([FromBody] MaintenanceRequestVM cvm, [FromQuery] Guid userId)
        {
            var request = new MaintenanceRequest
            {
                MaintenanceProId = cvm.MaintenanceProId,
                MaintenanceStatusId = cvm.MaintenanceStatusId,
                MaintenanceTypeId = cvm.MaintenanceTypeId,
                EmployeeId = cvm.EmployeeId,
                RequestDate = cvm.RequestDate,
                Description = cvm.Description,
                Location = cvm.Location,
                AssignedTo = cvm.AssignedTo,
                ScheduleDate = cvm.ScheduleDate,
                PriorityId = cvm.PriorityId,
                Verified = cvm.Verified
            };

            try
            {
                await _reqRepository.AddRequestAsync(request, userId); // Log the create action
                await _reqRepository.SaveChangesAsync();
                return Ok(request);
            }
            catch (Exception ex)
            {
                return BadRequest($"Internal Server Error: {ex.Message}");
            }
        }


        [HttpPut]
        [Route("EditRequest/{maintenanceReqId}")]
        public async Task<ActionResult<MaintenanceRequestVM>> EditRequest(Guid maintenanceReqId, [FromBody] MaintenanceRequestVM mvm, [FromQuery] Guid userId)
        {
            try
            {
                var existingReq = await _reqRepository.GetRequestAsync(maintenanceReqId);
                if (existingReq == null) return NotFound($"The maintenance request does not exist");

                var updatedReq = new MaintenanceRequest
                {
                    MaintenanceReqId = maintenanceReqId,
                    EmployeeId = mvm.EmployeeId,
                    MaintenanceProId = mvm.MaintenanceProId,
                    MaintenanceStatusId = mvm.MaintenanceStatusId,
                    MaintenanceTypeId = mvm.MaintenanceTypeId,
                    PriorityId = mvm.PriorityId,
                    RequestDate = mvm.RequestDate,
                    Description = mvm.Description,
                    Location = mvm.Location,
                    AssignedTo = mvm.AssignedTo,
                    ScheduleDate = mvm.ScheduleDate,
                    Verified = mvm.Verified
                };

                await _reqRepository.UpdateRequestAsync(updatedReq, existingReq, userId); // Log the update action
                await _reqRepository.SaveChangesAsync();

                return Ok(updatedReq);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }


        [HttpDelete]
        [Route("DeleteRequest/{maintenanceReqId}")]
        public async Task<IActionResult> DeleteRequest(Guid maintenanceReqId, [FromQuery] Guid userId)
        {
            try
            {
                var existingReq = await _reqRepository.GetRequestAsync(maintenanceReqId);
                if (existingReq == null) return NotFound($"The request does not exist");

                await _reqRepository.DeleteRequestAsync(existingReq, userId); // Log the delete action
                await _reqRepository.SaveChangesAsync();

                return Ok(existingReq);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }

    }
}
