using System.ComponentModel.DataAnnotations;

namespace EduProfileAPI.ViewModels.Maintenance
{
    public class MaintenancePriorityVM
    {
        public Guid PriorityId { get; set; }
        public string Description { get; set; }
    }
}
