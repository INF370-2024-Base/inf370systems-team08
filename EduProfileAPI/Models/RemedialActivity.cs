using System.ComponentModel.DataAnnotations;

namespace EduProfileAPI.Models
{
    public class RemedialActivity
    {
        [Key]
        public Guid RemActId { get; set; } = Guid.NewGuid();
        public Guid RemFileId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public byte[] Attachment { get; set; }
        public string? AttachmentName { get; set; }
        public string? AttachmentType { get; set; }
    }
}