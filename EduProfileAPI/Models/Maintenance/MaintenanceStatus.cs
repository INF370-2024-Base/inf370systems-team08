using System.ComponentModel.DataAnnotations;

namespace EduProfileAPI.Models.Maintenance
{
    public class MaintenanceStatus
    {
        [Key]
        public Guid MaintenanceStatusId { get; set; }
        public string Description { get; set; }
    }
}
