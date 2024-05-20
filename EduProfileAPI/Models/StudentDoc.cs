using System.ComponentModel.DataAnnotations;

namespace EduProfileAPI.Models
{
    public class StudentDoc
    {

        [Key]
        public Guid StuDocumentId { get; set; }
        public Guid StudentId { get; set; }
        public Guid DocumentTypeId { get; set; }

        //[Required]
        public string DocumentName { get; set; }
        public byte[] StudentDocumentAttachment { get; set; }
    }
}
