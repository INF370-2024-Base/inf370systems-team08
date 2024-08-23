namespace EduProfileAPI.ViewModels
{
    public class StudentDocVM
    {
        public Guid StudentId { get; set; }
        public Guid DocumentTypeId { get; set; }
        public string DocumentName { get; set; }
        public IFormFile StudentDocumentAttachment { get; set; }
        public string? AttachmentType { get; set; }

    }
}
