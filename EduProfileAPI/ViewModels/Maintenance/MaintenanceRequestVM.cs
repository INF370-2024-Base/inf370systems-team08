using System.Diagnostics.CodeAnalysis;

namespace EduProfileAPI.ViewModels.Maintenance
{
    public class MaintenanceRequestVM
    {
        public Guid MaintenanceReqId { get; set; }
        public Guid MaintenanceStatusId { get; set; }
        [AllowNull]
        public Guid MaintenanceTypeId { get; set; }
        [AllowNull]
        public Guid PriorityId { get; set; }
        [AllowNull]
        public Guid MaintenanceProId { get; set; }
        [AllowNull]

        public Guid? EmployeeId { get; set; }
        public DateOnly RequestDate { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public string AssignedTo { get; set; }
        public DateOnly ScheduleDate { get; set; }
    }
}
