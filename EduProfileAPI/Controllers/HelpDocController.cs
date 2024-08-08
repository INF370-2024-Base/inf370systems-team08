using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EduProfileAPI.Models;

namespace EduProfileAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HelpDocController : ControllerBase
    {
        // Hardcoded help document content
        public static string helpDocument = "Welcome to the EduProfile Help Document." +
                              "Here you will find all the information you need to navigate and use the system effectively. " +
                              "This document is designed to assist users in understanding the various features and functionalities of our system." +
                              "Getting Started:" +
                              "- How to log in:";

        // GET: api/helpdoc
        [HttpGet]
        [Route("GetHelpDoc")]
        public ActionResult<string> GetHelpDocument()
        {
            return Ok(helpDocument);
        }

        // PUT: api/helpdoc
        [HttpPut]
        public IActionResult UpdateHelpDocument([FromBody] HelpDoc model)
        {
            if (model == null || string.IsNullOrEmpty(model.Content))
            {
                return BadRequest("The help document content cannot be empty.");
            }

            helpDocument = model.Content;
            return Ok("Help document updated successfully.");
        }

    }
}
