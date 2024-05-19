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
        public EmployeeController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
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
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }


        [HttpGet]
        [Route("GetEmployee/{employeeId}")] //returns a specific course 
        public async Task<IActionResult> GetEmployeeAsync(int employeeId)
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
        public async Task<IActionResult> AddEmployee(CreateEmployeeVM cvm)
        {
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
                _employeeRepository.Add(employee);
                await _employeeRepository.SaveChangesAsync();
            }
            catch (Exception)
            {
                return BadRequest("Invalid transaction");
            }

            return Ok(employee);
        }

        [HttpPut]
        [Route("EditEmployee/{employeeId}")]
        public async Task<ActionResult<Employee>> EditEmployee(int employeeId, CreateEmployeeVM employeeModel)
        {
            try
            {
                var existingEmployee = await _employeeRepository.GetEmployeeAsync(employeeId);
                if (existingEmployee == null)
                    return NotFound($"The employee does not exist");

                existingEmployee.EmployeeId = employeeModel.EmployeeId;
                existingEmployee.EmployeeStatusId = employeeModel.EmployeeStatusId;
                existingEmployee.FirstName = employeeModel.FirstName;
                existingEmployee.LastName = employeeModel.LastName;
                existingEmployee.DateOfBirth = employeeModel.DateOfBirth;
                existingEmployee.Gender = employeeModel.Gender;
                existingEmployee.PhoneNumber = employeeModel.PhoneNumber;
                existingEmployee.Address = employeeModel.Address;
                existingEmployee.Salary = employeeModel.Salary;
                existingEmployee.IdentityNumber = employeeModel.IdentityNumber;

                if (await _employeeRepository.SaveChangesAsync())
                {
                    return Ok(existingEmployee);
                }
                else
                {
                    return StatusCode(500, "An error occurred while saving the employee.");
                }
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }

        [HttpDelete]
        [Route("DeleteEmployee/{employeeId}")]
        public async Task<IActionResult> DeleteEmployee(int employeeId)
        {
            try
            {
                var existingEmployee = await _employeeRepository.GetEmployeeAsync(employeeId);
                if (existingEmployee == null) return NotFound($"The employee does not exist");
                _employeeRepository.Delete(existingEmployee);

                if (await _employeeRepository.SaveChangesAsync()) return Ok(existingEmployee);
            }
            catch (Exception)
            {

                return StatusCode(500, "Internal Server Error. Please contact support.");
            }

            return BadRequest("Your request is invalid");
        }

    }
}
