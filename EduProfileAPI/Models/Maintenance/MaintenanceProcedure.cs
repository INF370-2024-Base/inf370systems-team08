using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace EduProfileAPI.Models.Maintenance
{
    public class MaintenanceProcedure
    {
        [Key]
        public Guid MaintenanceProId { get; set; }
        [AllowNull]
        public Guid? EmployeeId { get; set; }
        public string Description { get; set; }

        public DateTime CompletionDate { get; set; }
        public string Comments { get; set; }
        public string Costs { get; set; }
    }
}
