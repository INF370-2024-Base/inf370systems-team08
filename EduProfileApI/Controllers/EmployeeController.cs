using EduProfileAPI.Models;
using EduProfileAPI.Repositories.Interfaces;
using EduProfileAPI.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EduProfileAPI.Repositories.Implementation;
namespace EduProfileAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IAuditTrail _auditTrailRepository;  // Add audit trail repository

        public EmployeeController(IEmployeeRepository employeeRepository, IAuditTrail auditTrailRepository)
        {
            _employeeRepository = employeeRepository;
            _auditTrailRepository = auditTrailRepository;
        }

        [HttpGet]
        [Route("GetAllEmployees")] //returns a list of employees
        public async Task<IActionResult> GetAllEmployees()
        {
            try
            {
                var results = await _employeeRepository.GetAllEmployeesAsync();
                return Ok(results);
            }
            catch (Exception ex)
            {

                return StatusCode(500, $"Internal Server Error. Please contact support. {ex}");

            }
        }

        [HttpGet]
        [Route("GetEmployee/{employeeId}")] //returns a specific course 
        public async Task<IActionResult> GetEmployeeAsync(Guid employeeId)
        {
            try
            {
                var results = await _employeeRepository.GetEmployeeAsync(employeeId);

                if (results == null) return NotFound("Employee does not exist");

                return Ok(results);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }

        [HttpPost]
        [Route("AddEmployee")]
        public async Task<IActionResult> AddEmployee([FromBody] CreateEmployeeVM cvm, [FromQuery] Guid userId)
        {
            if (userId == Guid.Empty)
            {
                return BadRequest("User ID is required.");
            }

            var employee = new Employee
            {
                EmployeeStatusId = cvm.EmployeeStatusId,
                FirstName = cvm.FirstName,
                LastName = cvm.LastName,
                DateOfBirth = cvm.DateOfBirth,
                Gender = cvm.Gender,
                PhoneNumber = cvm.PhoneNumber,
                Address = cvm.Address,
                Salary = cvm.Salary,
                IdentityNumber = cvm.IdentityNumber
            };

            try
            {
                await _employeeRepository.AddEmployeeAsync(employee, userId);  // Log the create action
                await _employeeRepository.SaveChangesAsync();

                return Ok(employee);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpPut]
        [Route("EditEmployee/{employeeId}")]
        public async Task<IActionResult> EditEmployee(Guid employeeId, [FromBody] CreateEmployeeVM employeeModel, [FromQuery] Guid userId)
        {
            if (userId == Guid.Empty)
            {
                return BadRequest("User ID is required.");
            }

            try
            {
                var existingEmployee = await _employeeRepository.GetEmployeeAsync(employeeId);
                if (existingEmployee == null)
                    return NotFound("The employee does not exist");

                var updatedEmployee = new Employee
                {
                    EmployeeId = employeeId,
                    EmployeeStatusId = employeeModel.EmployeeStatusId,
                    FirstName = employeeModel.FirstName,
                    LastName = employeeModel.LastName,
                    DateOfBirth = employeeModel.DateOfBirth,
                    Gender = employeeModel.Gender,
                    PhoneNumber = employeeModel.PhoneNumber,
                    Address = employeeModel.Address,
                    Salary = employeeModel.Salary,
                    IdentityNumber = employeeModel.IdentityNumber
                };

                await _employeeRepository.UpdateEmployeeAsync(updatedEmployee, existingEmployee, userId);  // Log the update action
                await _employeeRepository.SaveChangesAsync();

                return Ok(updatedEmployee);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpDelete]
        [Route("DeleteEmployee/{employeeId}")]
        public async Task<IActionResult> DeleteEmployee(Guid employeeId, [FromQuery] Guid userId)
        {
            if (userId == Guid.Empty)
            {
                return BadRequest("User ID is required.");
            }

            try
            {
                var existingEmployee = await _employeeRepository.GetEmployeeAsync(employeeId);
                if (existingEmployee == null) return NotFound("The employee does not exist");

                await _employeeRepository.DeleteEmployeeAsync(existingEmployee, userId);  // Log the delete action
                await _employeeRepository.SaveChangesAsync();

                return Ok(existingEmployee);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

    }
}

