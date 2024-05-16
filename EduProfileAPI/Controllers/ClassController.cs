using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EduProfileAPI.DataAccessLayer;
using Microsoft.EntityFrameworkCore.Migrations;
using EduProfileAPI.Repositories.Interfaces;

namespace EduProfileAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassController : ControllerBase
    {
        private readonly EduProfileDbContext _context;
        private readonly IClass _classRepo;

        public ClassController(EduProfileDbContext context, IClass repository)
        {
            _context = context;
            _classRepo = repository;
        }

        [HttpGet]
        [Route("GetAllClasses")]
        public async Task<IActionResult> GetAllClassesAsync()
        {
            try
            {
                var results = await _classRepo.GetAllClassesAsync();
                return Ok(results);
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }
    }
}
