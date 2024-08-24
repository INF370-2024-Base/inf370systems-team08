using System.ComponentModel.DataAnnotations;

namespace EduProfileAPI.ViewModels
{
    public class IncidentTypeVm
    {

        [Required]
        [StringLength(100)]
        public string IncidentCategory { get; set; }

        [Required]
        [StringLength(50)]
        public string IncidentSeverity { get; set; }
    }
}
