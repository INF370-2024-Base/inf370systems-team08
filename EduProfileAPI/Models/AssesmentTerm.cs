using EduProfileAPI.Repositories.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EduProfileAPI.Models
{
    public class AssesmentTerm
    {
        [Key]
        public Guid TermId { get; set; }
        [Required]
        public Guid AssesmentId { get; set; }
        [Required]
        [Range(0, 4, ErrorMessage = "Term options only exist between 1-4")]
        public int Term { get; set; }
        [Required]
        [Range(0, 100, ErrorMessage = "Weighting must be between 0 and 100")]
        public decimal Weighting { get; set; }
        public ICollection<Assesment> Assesments { get; set; }
    }
}
