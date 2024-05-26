using EduProfileAPI.Models;
using EduProfileAPI.Repositories.Interfaces;
using EduProfileAPI.ViewModels;
using EduProfileAPI.EmailService;  
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace EduProfileAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentAnnouncementController : ControllerBase
    {
        private readonly IStudentAnnouncementRepo _studentAnnRepository;
        private readonly IEmailService _emailService;  

        public StudentAnnouncementController(IStudentAnnouncementRepo studentAnnRepository, IEmailService emailService)
        {
            _studentAnnRepository = studentAnnRepository;
            _emailService = emailService;  
        }

        [HttpPost]
        [Route("AddStudentAnnouncement")]
        public async Task<IActionResult> AddStudentAnnouncement(StudentAnnouncmentVM cvm)
        {
            var stuAnn = new StudentAnnouncement
            {
                ParentId = cvm.ParentId,
                AnnouncementDate = cvm.AnnouncementDate,
                Description = cvm.Description
            };

            try
            {
                _studentAnnRepository.Add(stuAnn);
                await _studentAnnRepository.SaveChangesAsync();

                
                if (!string.IsNullOrWhiteSpace(cvm.Email))
                {
                    var emailSubject = "New Student Announcement";
                    var emailBody = $"A new announcement has been made on {stuAnn.AnnouncementDate.ToString("MMMM dd, yyyy")}: {stuAnn.Description}";
                    await _emailService.SendEmailAsync("no-reply@eduprofileapi.com", cvm.Email, emailSubject, emailBody);
                }

                return Ok(stuAnn);
            }
            catch (Exception ex)
            {
                return BadRequest($"Invalid transaction: {ex.Message}");
            }
        }
    }
}
