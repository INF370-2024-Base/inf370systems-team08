using EduProfileAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace EduProfileAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GradeController : ControllerBase
    {
        private readonly IGradeRepository _gradeRepo;

        public GradeController(IGradeRepository gradeRepo)
        {
            _gradeRepo = gradeRepo;
        }

        [HttpGet]
        [Route("GetAllGrades")]
        public async Task<IActionResult> GetAllGradesAsync()
        {
            try
            {
                var results = await _gradeRepo.GetAllGradesAsync();
                return Ok(results);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Database Failure: {ex.Message}");
            }
        }
    }
}