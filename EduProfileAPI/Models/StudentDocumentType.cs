using System.ComponentModel.DataAnnotations;

namespace EduProfileAPI.Models
{
    public class StudentDocumentType
    {
        [Key]
        public Guid DocumentTypeId { get; set; }

        public string StudentDocumentTypeName { get; set; }
    }
}
