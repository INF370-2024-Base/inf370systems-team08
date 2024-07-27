using System.ComponentModel.DataAnnotations;

namespace EduProfileAPI.Models.Maintenance
{
    public class MaintenancePriority
    {
        [Key]
        public Guid PriorityId { get; set; }
        public string Description { get; set; }
    }
}
