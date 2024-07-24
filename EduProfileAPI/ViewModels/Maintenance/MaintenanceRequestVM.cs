using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace EduProfileAPI.ViewModels.Maintenance
{
    public class MaintenanceRequestVM
    {
        public Guid MaintenanceStatusId { get; set; }

        public Guid MaintenanceTypeId { get; set; }
        public Guid PriorityId { get; set; }
        public Guid? MaintenanceProId { get; set; }
        public Guid? EmployeeId { get; set; }

        public DateTime RequestDate { get; set; }

        [Required] // Ensure Description is not null
        public string Description { get; set; }

        [Required] // Ensure Location is not null
        public string Location { get; set; }

        [Required] // Ensure AssignedTo is not null
        public string AssignedTo { get; set; }

        public DateTime ScheduleDate { get; set; }
    }
}
