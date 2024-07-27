using System.Diagnostics.CodeAnalysis;

namespace EduProfileAPI.ViewModels.Maintenance
{
    public class MaintenanceProcedureVM
    {
        [AllowNull]
        public Guid? EmployeeId { get; set; }
        public string Description { get; set; }

        public DateTime CompletionDate { get; set; }
        public string Comments { get; set; }
        public string Costs { get; set; }
    }
}
