using EduProfileAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace EduProfileAPI.ViewModels
{
    public class AssesmentTermVM
    {
        public Guid AssesmentId { get; set; }
        public int Term { get; set; }
        public decimal Weighting { get; set; }
    }
}
