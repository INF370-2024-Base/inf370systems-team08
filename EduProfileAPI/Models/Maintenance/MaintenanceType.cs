using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace EduProfileAPI.Models.Maintenance
{
    public class MaintenanceType
    {
        [Key]
        public Guid MaintenanceTypeId { get; set; }
        public string Description { get; set; }
    }
}

