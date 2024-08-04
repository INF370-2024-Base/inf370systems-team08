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
    public class EmployeeStatusController : ControllerBase
    {
        private readonly IEmployeeStatusRepository _employeeStatusRepository;

        public EmployeeStatusController(IEmployeeStatusRepository employeeStatusRepository)
        {
            _employeeStatusRepository = employeeStatusRepository;
        }

        [HttpGet]
        [Route("GetAllEmployeeStatuses")]
        public async Task<IActionResult> GetAllEmployeeStatuses()
        {
            try
            {
                var results = await _employeeStatusRepository.GetAllEmployeeStatusesAsync();
                return Ok(results);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }

        [HttpGet]
        [Route("GetEmployeeStatus/{employeeStatusId}")]
        public async Task<IActionResult> GetEmployeeStatusAsync(Guid employeeStatusId)
        {
            try
            {
                var result = await _employeeStatusRepository.GetEmployeeStatusAsync(employeeStatusId);
                if (result == null) return NotFound("Employee status does not exist");
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }

        [HttpPost]
        [Route("AddEmployeeStatus")]
        public async Task<IActionResult> AddEmployeeStatus(CreateEmployeeStatusVM statusVM)
        {
            var employeeStatus = new EmployeeStatus
            {
                EmployeeStatusId = statusVM.EmployeeStatusId,
                Description = statusVM.Description
            };

            try
            {
                _employeeStatusRepository.Add(employeeStatus);
                await _employeeStatusRepository.SaveChangesAsync();
                return Ok(employeeStatus);
            }
            catch (Exception)
            {
                return BadRequest("Invalid transaction");
            }
        }

        [HttpPut]
        [Route("EditEmployeeStatus/{employeeStatusId}")]
        public async Task<IActionResult> EditEmployeeStatus(Guid employeeStatusId, CreateEmployeeStatusVM statusVM)
        {
            try
            {
                var existingStatus = await _employeeStatusRepository.GetEmployeeStatusAsync(employeeStatusId);
                if (existingStatus == null)
                    return NotFound("The employee status does not exist");

                existingStatus.Description = statusVM.Description;

                if (await _employeeStatusRepository.SaveChangesAsync())
                {
                    return Ok(existingStatus);
                }
                else
                {
                    return StatusCode(500, "An error occurred while saving the employee status.");
                }
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }

        [HttpDelete]
        [Route("DeleteEmployeeStatus/{employeeStatusId}")]
        public async Task<IActionResult> DeleteEmployeeStatus(Guid employeeStatusId)
        {
            try
            {
                var existingStatus = await _employeeStatusRepository.GetEmployeeStatusAsync(employeeStatusId);
                if (existingStatus == null) return NotFound("The employee status does not exist");

                _employeeStatusRepository.Delete(existingStatus);

                if (await _employeeStatusRepository.SaveChangesAsync())
                {
                    return Ok(existingStatus);
                }
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }

            return BadRequest("Your request is invalid");
        }
    }
}
