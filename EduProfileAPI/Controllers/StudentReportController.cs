using EduProfileAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EduProfileAPI.EmailService;
using EduProfileAPI.ViewModels;
using System.Text;

namespace EduProfileAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentReportController : ControllerBase
    {
        private readonly IStudentReportRepository _studentReportRepository;
        private readonly IEmailService _emailService;

        public StudentReportController(IStudentReportRepository studentReportRepository, IEmailService emailService)
        {
            _studentReportRepository = studentReportRepository;
            _emailService = emailService;
        }

        [HttpPost("SendProgressReport")]
        public async Task<IActionResult> SendProgressReport([FromBody] SendProgressReportRequest request)
        {
            var report = await _studentReportRepository.GetStudentProgressReportAsync(request.StudentId);
            if (report == null)
            {
                return NotFound();
            }

            var emailBody = GenerateEmailBody(report);
            await _emailService.SendEmailAsync("smaduna02@gmail.com", report.ParentEmail, "Student Progress Report", emailBody);

            return Ok(new { Message = "Progress report sent successfully." });
        }

        private string GenerateEmailBody(StudentProgressReportViewModel report)
        {
            var builder = new StringBuilder();
            builder.AppendLine($"<h1>Progress Report for {report.StudentName}</h1>");
            if (report.Assessments != null && report.Assessments.Any())
            {
                builder.AppendLine("<h2>Assessments</h2>");
                foreach (var assessmentReport in report.Assessments)
                {
                    var assessment = assessmentReport.Assesment;
                    builder.AppendLine($"<p>{assessment.AssesmentName} - {assessment.AssesmentType} on {assessment.AssesmentDate.ToShortDateString()}: {assessmentReport.MarkAchieved}/{assessment.AchievableMark}</p>");
                }
            }
            if (report.Merits != null && report.Merits.Any())
            {
                builder.AppendLine("<h2>Merits</h2>");
                foreach (var merit in report.Merits)
                {
                    builder.AppendLine($"<p>{merit.MeritName}: {merit.MeritDescription}</p>");
                }
            }
            if (report.Incidents != null && report.Incidents.Any())
            {
                builder.AppendLine("<h2>Incidents</h2>");
                foreach (var incident in report.Incidents)
                {
                    builder.AppendLine($"<p>On {incident.IncidentDate.ToShortDateString()}: {incident.IncidentDescription}</p>");
                }
            }

            return builder.ToString();
        }
        public class SendProgressReportRequest
        {
            public Guid StudentId { get; set; }
            
        }
    }
}
