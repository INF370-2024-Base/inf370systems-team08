using EduProfileAPI.Models;
using EduProfileAPI.Repositories.Interfaces;
using EduProfileAPI.ViewModels;
using EduProfileAPI.EmailService;  
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using EduProfileAPI.Repositories.Implementation;

namespace EduProfileAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentAnnouncementController : ControllerBase
    {
        private readonly IStudentAnnouncementRepo _studentAnnRepository;
        private readonly IEmailService _emailService;  
        private readonly IStudentRepository _studentRepository;

        public StudentAnnouncementController(IStudentAnnouncementRepo studentAnnRepository, IEmailService emailService, IStudentRepository studentRepository)
        {
            _studentAnnRepository = studentAnnRepository;
            _emailService = emailService;  
            _studentRepository = studentRepository;
        }

        [HttpPost]
        [Route("AddStudentAnnouncement")]
        public async Task<IActionResult> AddStudentAnnouncement(StudentAnnouncmentVM cvm)
        {

            var stuAnn = new StudentAnnouncement
            {
                ParentId = Guid.Parse(await _studentRepository.GetRandomParentIdAsync()),
                AnnouncementDate = cvm.AnnouncementDate,
                Description = cvm.Description
            };

            try
            {
                _studentAnnRepository.Add(stuAnn);
                await _studentAnnRepository.SaveChangesAsync();

                
                if (cvm.Emails != null && cvm.Emails.Count > 0)
                {
                    //var emailSubject = "New Student Announcement";
                    //var emailBody = $"A new announcement has been made on {stuAnn.AnnouncementDate.ToString("MMMM dd, yyyy")}: {stuAnn.Description}";
                    //await _emailService.SendEmailAsync("no-reply@eduprofileapi.com", cvm.Emails, emailSubject, emailBody);
                    var emailSubject = "New Student Announcement";
                    var emailBody = $"A new announcement has been made on {stuAnn.AnnouncementDate.ToString("MMMM dd, yyyy")}: {stuAnn.Description}";

                    // Send an email to each address
                    foreach (var email in cvm.Emails)
                    {
                        await _emailService.SendEmailAsync("no-reply@eduprofileapi.com", email, emailSubject, emailBody);
                    }
                }

                return Ok(stuAnn);
            }
            catch (Exception ex)
            {
                return BadRequest($"Invalid transaction: {ex.Message}");
            }
        }

        //[HttpPost]
        //[Route("AddStudentAnnouncement")]
        //public async Task<IActionResult> AddStudentAnnouncement(StudentAnnouncmentVM cvm)
        //{
        //    var stuAnn = new StudentAnnouncement
        //    {
        //        ParentId = cvm.ParentId,
        //        AnnouncementDate = cvm.AnnouncementDate,
        //        Description = cvm.Description
        //    };

        //    try
        //    {
        //        _studentAnnRepository.Add(stuAnn);
        //        await _studentAnnRepository.SaveChangesAsync();


        //        if (!string.IsNullOrWhiteSpace(cvm.Email))
        //        {
        //            var emailSubject = "New Student Announcement";
        //            var emailBody = $"A new announcement has been made on {stuAnn.AnnouncementDate.ToString("MMMM dd, yyyy")}: {stuAnn.Description}";
        //            await _emailService.SendEmailAsync("no-reply@eduprofileapi.com", cvm.Email, emailSubject, emailBody);
        //        }

        //        return Ok(stuAnn);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest($"Invalid transaction: {ex.Message}");
        //    }
        //}
    }
}
