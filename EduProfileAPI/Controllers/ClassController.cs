using EduProfileAPI.Models;
using EduProfileAPI.Repositories.Interfaces;
using EduProfileAPI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using EduProfileAPI.DataAccessLayer;

[Route("api/[controller]")]
[ApiController]
public class ClassController : ControllerBase
{
    private readonly IClass _ClassRepo;
    private readonly EduProfileDbContext _context;

    public ClassController(IClass ClassRepo, EduProfileDbContext context)
    {
        _ClassRepo = ClassRepo;
        _context = context;
    }

    [HttpGet]
    [Route("GetAllClasses")]
    public async Task<IActionResult> GetAllClasses()
    {
        var classes = await _ClassRepo.GetAllClassesAsync();
        return Ok(classes);
    }

    [HttpGet("GetClassByEmployeeId/{employeeId}")]
    public async Task<IActionResult> GetClassByEmployeeId(Guid employeeId)
    {
        try
        {
            var results = await _context.Class.FindAsync(employeeId);

            if (results == null) return NotFound("Class does not exist");

            return Ok(results);
        }
        catch (Exception)
        {
            return StatusCode(500, "Internal Server Error. Please contact support");
        }
    }


    [HttpGet]
    [Route("GetClasses/{classId}")]
    public async Task<IActionResult> GetClassAsync(Guid classId)
    {
        try
        {
            var results = await _ClassRepo.GetClassAsync(classId);

            if (results == null) return NotFound("Class does not exist");

            return Ok(results);
        }
        catch (Exception)
        {
            return StatusCode(500, "Internal Server Error. Please contact support");
        }
    }

    [HttpPost]
    [Route("AddClass")]
    public async Task<IActionResult> AddClass([FromBody] ClassVM cvm, [FromQuery] Guid userId)
    {
        var newClass = new Class
        {
            GradeId = cvm.GradeId,
            EmployeeId = cvm.EmployeeId,
            ClassName = cvm.ClassName,
            ClassDescription = cvm.ClassDescription
        };

        try
        {
            await _ClassRepo.AddClassAsync(newClass, userId); // Log the create action
            await _ClassRepo.SaveChangesAsync();
            return Ok(newClass);
        }
        catch (Exception ex)
        {
            return BadRequest($"Internal Server Error: {ex.Message}");
        }
    }

    // Edit class
    [HttpPut]
    [Route("EditClass/{classId}")]
    public async Task<IActionResult> EditClass(Guid classId, [FromBody] ClassVM classVM, [FromQuery] Guid userId)
    {
        try
        {
            var existingClass = await _ClassRepo.GetClassAsync(classId);
            if (existingClass == null) return NotFound("The class does not exist");

            var updatedClass = new Class
            {
                ClassId = classId,
                GradeId = classVM.GradeId,
                EmployeeId = classVM.EmployeeId,
                ClassName = classVM.ClassName,
                ClassDescription = classVM.ClassDescription
            };

            await _ClassRepo.UpdateClassAsync(updatedClass, existingClass, userId); // Log the update action
            await _ClassRepo.SaveChangesAsync();

            return Ok(updatedClass);
        }
        catch (Exception)
        {
            return StatusCode(500, "Internal Server Error. Please contact support.");
        }
    }

    // Delete class
    [HttpDelete]
    [Route("DeleteClass/{classId}")]
    public async Task<IActionResult> DeleteClass(Guid classId, [FromQuery] Guid userId)
    {
        try
        {
            var existingClass = await _ClassRepo.GetClassAsync(classId);
            if (existingClass == null) return NotFound("The class does not exist");

            await _ClassRepo.DeleteClassAsync(existingClass, userId); // Log the delete action
            await _ClassRepo.SaveChangesAsync();

            return Ok(existingClass);
        }
        catch (Exception)
        {
            return StatusCode(500, "Internal Server Error. Please contact support.");
        }
    }

}
