using System.ComponentModel.DataAnnotations;

namespace EduProfileAPI.Models
{
    public class IncidentType
    {
            [Key]
            public Guid IncidentTypeId { get; set; }

            [Required]
            [StringLength(100)]
            public string IncidentCategory { get; set; }

            [Required]
            [StringLength(50)]
            public string IncidentSeverity { get; set; }
    }
}
