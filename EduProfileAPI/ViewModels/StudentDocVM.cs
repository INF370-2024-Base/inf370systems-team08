namespace EduProfileAPI.ViewModels
{
    public class StudentDocVM
    {
        public Guid StuDocumentId { get; set; }
        public Guid StudentId { get; set; }
        public Guid DocumentTypeId { get; set; }
        public string DocumentName { get; set; }
        public byte[] StudentDocumentAttachment { get; set; }
    }
}
