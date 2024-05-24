using System.ComponentModel.DataAnnotations;

namespace EduProfileAPI.Models
{
    public class Merit
    {
        [Key]
        public Guid MeritId { get; set; }
        public Guid MeritTypeId { get; set; }
        public Guid EmployeeId { get; set; }
        public Guid StudentId { get; set; }


        //[Required]
        public string MeritName { get; set; }
        public string MeritDescription { get; set; }



    }
}
