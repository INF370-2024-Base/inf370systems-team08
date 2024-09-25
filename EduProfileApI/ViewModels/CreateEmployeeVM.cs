using System.ComponentModel.DataAnnotations;

namespace EduProfileAPI.ViewModels
{
    public class CreateEmployeeVM
    {

        public Guid EmployeeStatusId { get; set; }
        [StringLength(30)]
        public string FirstName { get; set; }

        [StringLength(30)]
        public string LastName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string Gender { get; set; }

        [StringLength(10)]
        public string PhoneNumber { get; set; }

        [StringLength(50)]
        public string Address { get; set; }

        [StringLength(20)]
        public string Salary { get; set; }

        [StringLength(13)]
        public string IdentityNumber { get; set; }
    }
}
