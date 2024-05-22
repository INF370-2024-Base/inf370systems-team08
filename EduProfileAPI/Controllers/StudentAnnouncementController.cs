using EduProfileAPI.Models;
using EduProfileAPI.Repositories.Interfaces;
using EduProfileAPI.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EduProfileAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentAnnouncementController : ControllerBase
    {
        private readonly IStudentAnnouncementRepo _studentAnnRepository;

        public StudentAnnouncementController(IStudentAnnouncementRepo studentAnnRepository)
        {
            _studentAnnRepository = studentAnnRepository;
        }
        [HttpPost]
        [Route("AddStudentAnnouncement")]
        public async Task<IActionResult> AddStudentAnnouncement(StudentAnnouncmentVM cvm)
        {
            var stuAnn = new StudentAnnouncement { ParentId = cvm.ParentId, AnnouncementDate = cvm.AnnouncementDate, Description = cvm.Description };

            try
            {
                _studentAnnRepository.Add(stuAnn);
                await _studentAnnRepository.SaveChangesAsync();
            }
            catch (Exception)
            {
                return BadRequest("Invalid transaction");
            }

            return Ok(stuAnn);
        }
    }
}
