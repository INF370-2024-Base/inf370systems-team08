using EduProfileAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace EduProfileAPI.ViewModels
{
    public class AssesmentTermVM
    {
        public int Term { get; set; }
        public decimal Weighting { get; set; }
        public Guid SubjectId { get; set; }
    }
}
