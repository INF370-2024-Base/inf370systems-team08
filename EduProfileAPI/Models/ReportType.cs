using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace EduProfileAPI.Models
{
    public class ReportType
    {
        [Key]
        public Guid ReportTypeId { get; set; }

        public string ReportTypeName { get; set; }
    }
}
