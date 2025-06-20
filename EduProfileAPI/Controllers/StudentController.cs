﻿using EduProfileAPI.Models;
using EduProfileAPI.Repositories.Interfaces;
using EduProfileAPI.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduProfileAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IAuditTrail _auditTrailRepository;  // Add audit trail repository


        public StudentController(IStudentRepository studentRepository, IAuditTrail auditTrailRepository)
        {
            _studentRepository = studentRepository;
            _auditTrailRepository = auditTrailRepository;
        }

        [HttpGet]
        [Route("GetAllStudents")] 
        public async Task<IActionResult> GetAllStudents()
        {
            try
            {
                var results = await _studentRepository.GetAllStudentsAsync();
                return Ok(results);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error. Please contact support. Error details: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("GetStudent/{studentId}")] // returns a specific merit
        public async Task<IActionResult> GetStudentAsync(Guid studentId)
        {
            try
            {
                var results = await _studentRepository.GetStudentAsync(studentId);

                if (results == null) return NotFound("Student does not exist");

                return Ok(results);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }

        [HttpPost]
        [Route("AddStudent")]
        public async Task<IActionResult> AddStudent([FromBody] StudentVM cvm, [FromQuery] Guid userId)
        {
            if (userId == Guid.Empty)
            {
                return BadRequest("User ID is required.");
            }

            var student = new Student
            {
                GradeId = cvm.GradeId,
                ClassId = cvm.ClassId,
                ParentId = cvm.ParentId,
                FirstName = cvm.FirstName,
                LastName = cvm.LastName,
                DateOfBirth = cvm.DateOfBirth,
                Gender = cvm.Gender,
                Address = cvm.Address,
                AdmissionNo = cvm.AdmissionNo,
                EmergencyContactName = cvm.EmergencyContactName,
                EmergencyContactRelationship = cvm.EmergencyContactRelationship,
                EmergencyContactPhoneNum = cvm.EmergencyContactPhoneNum
            };

            try
            {
                await _studentRepository.AddStudentAsync(student, userId); // Log the creation in audit trail
                await _studentRepository.SaveChangesAsync();

                return Ok(student);
            }
            catch (Exception)
            {
                return BadRequest("Invalid transaction");
            }
        }

        [HttpPut]
        [Route("EditStudent/{studentId}")]
        public async Task<ActionResult<StudentVM>> EditStudent(Guid studentId, [FromBody] StudentVM studentModel, [FromQuery] Guid userId)
        {
            if (userId == Guid.Empty)
            {
                return BadRequest("User ID is required.");
            }

            try
            {
                var existingStudent = await _studentRepository.GetStudentAsync(studentId);
                if (existingStudent == null) return NotFound("The student does not exist");

                var updatedStudent = new Student
                {
                    StudentId = studentId,
                    GradeId = studentModel.GradeId,
                    ClassId = studentModel.ClassId,
                    ParentId = studentModel.ParentId,
                    FirstName = studentModel.FirstName,
                    LastName = studentModel.LastName,
                    DateOfBirth = studentModel.DateOfBirth,
                    Gender = studentModel.Gender,
                    Address = studentModel.Address,
                    AdmissionNo = studentModel.AdmissionNo,
                    EmergencyContactName = studentModel.EmergencyContactName,
                    EmergencyContactRelationship = studentModel.EmergencyContactRelationship,
                    EmergencyContactPhoneNum = studentModel.EmergencyContactPhoneNum
                };

                await _studentRepository.UpdateStudentAsync(updatedStudent, existingStudent, userId); // Log the update in audit trail
                await _studentRepository.SaveChangesAsync();

                return Ok(updatedStudent);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }

        [HttpDelete]
        [Route("DeleteStudent/{studentId}")]
        public async Task<IActionResult> DeleteStudent(Guid studentId, [FromQuery] Guid userId)
        {
            if (userId == Guid.Empty)
            {
                return BadRequest("User ID is required.");
            }

            try
            {
                var existingStudent = await _studentRepository.GetStudentAsync(studentId);
                if (existingStudent == null) return NotFound("The student does not exist");

                await _studentRepository.DeleteStudentAsync(existingStudent, userId); // Log the delete action
                await _studentRepository.SaveChangesAsync();

                return Ok(existingStudent);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }

        [HttpGet]
        [Route("GetParents")]
        public async Task<ActionResult<Parent[]>> GetAllParents()
        {
            var parents = await _studentRepository.GetAllParentsAsync();
            return Ok(parents);
        }

        //Get parent by ID
        [HttpGet("GetParent/{parentId}")]
        public async Task<IActionResult> GetParent(Guid parentId)
        {
            try
            {
                var parent = await _studentRepository.GetParentAsync(parentId);
                if (parent == null)
                {
                    return NotFound();
                }
                return Ok(parent);
            }
            catch (Exception ex)
            {
                // Handle the exception as needed
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // Add Parent
        [HttpPost("AddParent")]
        public async Task<IActionResult> AddParent([FromBody] Parent parent)
        {
            try
            {
                if (parent == null)
                {
                    return BadRequest("Parent is null.");
                }

                _studentRepository.Add(parent);
                if (await _studentRepository.SaveChangesAsync())
                {
                    return CreatedAtAction(nameof(GetParent), new { parentId = parent.ParentId }, parent);
                }

                return BadRequest("Failed to add parent.");
            }
            catch (Exception ex)
            {
                // Handle the exception as needed
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // Edit Parent
        [HttpPut("EditParent/{parentId}")]
        public async Task<IActionResult> EditParent(Guid parentId, [FromBody] Parent updatedParent)
        {
            try
            {
                if (updatedParent == null || parentId != updatedParent.ParentId)
                {
                    return BadRequest("Parent data is invalid.");
                }

                var parent = await _studentRepository.GetParentAsync(parentId);
                if (parent == null)
                {
                    return NotFound();
                }

                // Update the parent properties
                parent.Parent1Name = updatedParent.Parent1Name;
                parent.Parent1MaritalStatus = updatedParent.Parent1MaritalStatus;
                parent.Parent1Occupation = updatedParent.Parent1Occupation;
                parent.Parent1PhysicalAddress = updatedParent.Parent1PhysicalAddress;
                parent.Parent1PostalAddress = updatedParent.Parent1PostalAddress;
                parent.Parent1HomePhone = updatedParent.Parent1HomePhone;
                parent.Parent1WorkPhone = updatedParent.Parent1WorkPhone;
                parent.Parent1CellPhone = updatedParent.Parent1CellPhone;
                parent.Parent2Name = updatedParent.Parent2Name;
                parent.Parent2MaritalStatus = updatedParent.Parent2MaritalStatus;
                parent.Parent2Occupation = updatedParent.Parent2Occupation;
                parent.Parent2PhysicalAddress = updatedParent.Parent2PhysicalAddress;
                parent.Parent2PostalAddress = updatedParent.Parent2PostalAddress;
                parent.Parent2HomePhone = updatedParent.Parent2HomePhone;
                parent.Parent2WorkPhone = updatedParent.Parent2WorkPhone;
                parent.Parent2CellPhone = updatedParent.Parent2CellPhone;

                if (await _studentRepository.SaveChangesAsync())
                {
                    return NoContent();
                }

                return BadRequest("Failed to update parent.");
            }
            catch (Exception ex)
            {
                // Handle the exception as needed
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpDelete("DeleteParent/{parentId}")]
        public async Task<IActionResult> DeleteParent(Guid parentId)
        {
            try
            {
                // Retrieve the parent entity
                var parent = await _studentRepository.GetParentAsync(parentId);
                if (parent == null)
                {
                    return NotFound();
                }

                // Check if there are any students associated with the parent
                var students = await _studentRepository.GetStudentsByParentIdAsync(parentId);
                if (students.Any())
                {
                    return BadRequest("Cannot delete parent. There are students associated with this parent.");
                }

                // Proceed to delete the parent entity
                _studentRepository.Delete(parent);
                if (await _studentRepository.SaveChangesAsync())
                {
                    return NoContent();
                }

                return BadRequest("Failed to delete parent.");
            }
            catch (DbUpdateException dbEx)
            {
                // Log the detailed error
                var innerException = dbEx.InnerException?.Message ?? dbEx.Message;
                return StatusCode(500, $"Internal server error: {innerException}");
            }
            catch (Exception ex)
            {
                // Handle the exception as needed
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("parent-emails")]
        public async Task<ActionResult<List<ParentEmailVM>>> GetParentEmails()
        {
            try
            {
                var emails = await _studentRepository.GetAllParentEmailsAsync();
                if (emails == null || emails.Count == 0)
                    return NotFound("No parent emails found.");

                return Ok(emails);
            }
            catch (Exception ex)
            {
                // Log the exception details
                return StatusCode(500, "An error occurred while retrieving parent emails.");
            }
        }

    }
}

