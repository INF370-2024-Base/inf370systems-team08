using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EduProfileAPI.Models;

namespace EduProfileAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HelpDocController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetHelpDocument()
        {
            // Combine paths to get the full path to the text file
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Documents", "Help.txt");

            // Check if the file exists
            if (System.IO.File.Exists(filePath))
            {
                // Read the content of the file
                var content = await System.IO.File.ReadAllTextAsync(filePath);
                return Ok(content); // Return the content to the client
            }

            // Return a 404 if the file is not found
            return NotFound("Help document not found.");
        }

    }
}
