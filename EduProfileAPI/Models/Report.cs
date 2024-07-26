using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace EduProfileAPI.Models
{
    public class Report
    {

        [Key]
        public Guid ReportId { get; set; }
        public Guid ReportTypeId { get; set; }
        [AllowNull]
        public Guid? UserId { get; set; }
        public string Title { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Description { get; set; }
        public byte[] ReportAttachment { get; set; }



    }
}
