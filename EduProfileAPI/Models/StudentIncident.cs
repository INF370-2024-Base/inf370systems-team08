using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Org.BouncyCastle.Asn1.Cms;
using System.ComponentModel.DataAnnotations;

namespace EduProfileAPI.Models
{
    public class StudentIncident
    {
        [Key]
            public Guid IncidentId { get; set; }
        [Required]
            public Guid StudentId { get; set; }
        [Required]
            public Guid IncidentTypeId { get; set; }
            public DateTime IncidentDate { get; set; }
            public TimeSpan? IncidentTime { get; set; }
            public string? IncidentLocation { get; set; }
            public string? IncidentDescription { get; set; }
            public string? ReportedBy { get; set; }
            public DateTime? ReportedDate { get; set; }
            public string? IncidentStatus { get; set; }
            public bool? ParentNotified { get; set; }
            public string? Comments { get; set; }
            public byte[]? IncidentAttachment { get; set; }
    }
}
