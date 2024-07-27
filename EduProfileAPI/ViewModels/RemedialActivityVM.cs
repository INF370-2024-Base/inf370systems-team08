namespace EduProfileAPI.ViewModels
{
    public class RemedialActivityVM
    {
        public Guid RemFileId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public IFormFile Attachment { get; set; }
        public string? AttachmentName { get; set; }
        public string? AttachmentType { get; set; }
    }
}
