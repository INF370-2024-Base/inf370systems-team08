using System.Diagnostics.CodeAnalysis;

namespace EduProfileAPI.ViewModels
{
    public class ReportVM
    {
        public Guid ReportTypeId { get; set; }
        [AllowNull]
        public Guid? UserId { get; set; }

        public string Title { get; set; }
        public DateTime CreatedDate { get; set; }

        public string Description { get; set; }
        public IFormFile ReportAttachment { get; set; }
    }
}
