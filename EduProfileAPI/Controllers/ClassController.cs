using EduProfileAPI.Models.Class;
using EduProfileAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class ClassController : ControllerBase
{
    private readonly IClass _classRepo;

    public ClassController(IClass classRepo)
    {
        _classRepo = classRepo;
    }

    [HttpGet("GetAllClasses")]
    public async Task<IActionResult> GetAllClassesAsync()
    {
        try
        {
            var classes = await _classRepo.GetAllClassesAsync();
            return Ok(classes);
        }
        catch (Exception)
        {
            // Log the exception
            return StatusCode(500, "Internal Server Error. Please contact support.");
        }
    }

    [HttpGet("GetClass/{classId}")]
    public async Task<IActionResult> GetClassAsync(Guid classId)
    {
        try
        {
            var classItem = await _classRepo.GetClassAsync(classId);
            if (classItem == null) return NotFound("Class does not exist");

            return Ok(classItem);
        }
        catch (Exception)
        {
            // Log the exception
            return StatusCode(500, "Internal Server Error. Please contact support.");
        }
    }

    [HttpPost("AddClass")]
    public async Task<IActionResult> AddClass(ClassVM classVM)
    {
        var newClass = new Class
        {
            ClassId = classVM.ClassId,
            GradeId = classVM.GradeId,
            EmployeeId = classVM.EmployeeId,
            ClassName = classVM.ClassName,
            ClassDescription = classVM.ClassDescription
        };

        try
        {
            _classRepo.Add(newClass);
            if (await _classRepo.SaveChangesAsync())
            {
                return CreatedAtAction(nameof(GetClassAsync), new { classId = newClass.ClassId }, newClass);
            }
        }
        catch (Exception)
        {
            // Log the exception
            return BadRequest("Invalid transaction");
        }

        return BadRequest("Failed to save new class");
    }

    [HttpPut("EditClass/{classId}")]
    public async Task<IActionResult> UpdateClass(Guid classId, ClassVM classVM)
    {
        try
        {
            var classToUpdate = await _classRepo.GetClassAsync(classId);
            if (classToUpdate == null)
            {
                return NotFound($"The class with ID {classId} does not exist.");
            }

            classToUpdate.GradeId = classVM.GradeId;
            classToUpdate.EmployeeId = classVM.EmployeeId;
            classToUpdate.ClassName = classVM.ClassName;
            classToUpdate.ClassDescription = classVM.ClassDescription;

            if (await _classRepo.SaveChangesAsync())
            {
                return Ok(classToUpdate);
            }
        }
        catch (Exception)
        {
            // Log the exception
            return StatusCode(500, "Internal Server Error. Please contact support.");
        }

        return BadRequest("Update failed. No changes were made.");
    }

    [HttpDelete("DeleteClass/{classId}")]
    public async Task<IActionResult> DeleteClass(Guid classId)
    {
        try
        {
            var classToDelete = await _classRepo.GetClassAsync(classId);
            if (classToDelete == null) return NotFound($"The class does not exist");

            _classRepo.Delete(classToDelete);

            if (await _classRepo.SaveChangesAsync())
            {
                return NoContent();
            }
        }
        catch (Exception)
        {
            // Log the exception
            return StatusCode(500, "Internal Server Error. Please contact support.");
        }

        return BadRequest("Delete request is invalid");
    }
}
