using System.ComponentModel.DataAnnotations;
namespace EduProfileAPI.Models
{
    public class Employee
    {
        [Key]
        public Guid EmployeeId { get; set; }

        public Guid EmployeeStatusId { get; set; }
        [StringLength(30)]
        public string FirstName { get; set; }

        [StringLength(30)]
        public string LastName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public bool Gender { get; set; }

        [StringLength(10)]
        public string PhoneNumber { get; set; }

        [StringLength(20)]
        public string Address { get; set; }

        [StringLength(20)]
        public string Salary { get; set; }

        [StringLength(13)]
        public string IdentityNumber { get; set; }
    }
}

