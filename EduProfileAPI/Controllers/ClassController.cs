using EduProfileAPI.Models;
using EduProfileAPI.Repositories.Interfaces;
using EduProfileAPI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

[Route("api/[controller]")]
[ApiController]
public class ClassController : ControllerBase
{
    private readonly IClass _ClassRepo;

    public ClassController(IClass ClassRepo)
    {
        _ClassRepo = ClassRepo;
    }

    [HttpGet]
    [Route("GetAllClasses")]
    public async Task<IActionResult> GetAllClasses()
    {
        try
        {
            var result = await _ClassRepo.GetAllClassesAsync();
            return Ok(result);
        }
        catch (Exception)
        {
            return StatusCode(500, "Internal Server Error. Please contact support.");
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
    public async Task<IActionResult> AddClass(ClassVM cvm)
    {
        var classes = new Class { ClassId = cvm.ClassId, GradeId = cvm.GradeId, EmployeeId = cvm.EmployeeId, ClassName = cvm.ClassName, ClassDescription = cvm.ClassDescription };

        try
        {
            _ClassRepo.Add(classes);
            await _ClassRepo.SaveChangesAsync();
        }
        catch (Exception)
        {
            return BadRequest("Invalid transaction");
        }

        return Ok(classes);
    }

    [HttpPut]
    [Route("EditClass/{classId}")]
    public async Task<ActionResult<ClassVM>> EditClass(Guid classId, ClassVM classVM)
    {

        try
        {
            var existingClass = await _ClassRepo.GetClassAsync(classId);

            if (existingClass == null) return NotFound($"The class does not exist");
            existingClass.GradeId = classVM.GradeId;
            existingClass.EmployeeId = classVM.EmployeeId;
            existingClass.ClassName = classVM.ClassName;
            existingClass.ClassDescription = classVM.ClassDescription;

            if(await _ClassRepo.SaveChangesAsync())
            {
                return Ok(existingClass);
            }
        }
        catch (Exception)
        {
            return StatusCode(500, "Internal Server Error. Please contact support.");
        }

        return BadRequest("Your request is invalid.");

    }

    [HttpDelete]
    [Route("DeleteClass/{classId}")]
    public async Task<IActionResult> DeleteClass(Guid classId)
    {

        try
        {
            var existingClass = await _ClassRepo.GetClassAsync(classId);
            if (existingClass == null) return NotFound($"The class does not exist");
            _ClassRepo.Delete(existingClass);

            if (await _ClassRepo.SaveChangesAsync()) return Ok(existingClass); 
        }
        catch (Exception)
        {
            return StatusCode(500, "Internal Server Error. Please contact support.");
        }
        return BadRequest("Your request is invalid");
    }
}
