using System.ComponentModel.DataAnnotations;

namespace EduProfileAPI.Models
{
    public class EarlyReleases
    {
        [Key]
        public Guid EarlyRelId { get; set; }
        public Guid StudentId { get; set; }
        public Guid ParentId { get; set; }
        public Guid EmployeeId { get; set; }
        public DateTime DateOfEarlyRelease { get; set; }
        public string ReasonForEarlyRelease { get; set; }
        public string SignerRelationship { get; set; }
        public string SignerName { get; set;}
        public string SignerIDNumber { get; set; }
    }
}
