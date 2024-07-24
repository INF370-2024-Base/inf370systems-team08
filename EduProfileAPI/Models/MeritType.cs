using System.ComponentModel.DataAnnotations;

namespace EduProfileAPI.Models
{
    public class MeritType
    {
        [Key]
        public Guid MeritTypeId { get; set; }
        public string MeritTypeName { get; set; }
    }
}
