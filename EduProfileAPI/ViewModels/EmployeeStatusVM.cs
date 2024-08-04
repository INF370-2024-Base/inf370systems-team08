using System.ComponentModel.DataAnnotations;

namespace EduProfileAPI.ViewModels
{
    public class CreateEmployeeStatusVM
    {
        public Guid EmployeeStatusId { get; set; }

        [StringLength(50)]
        public string Description { get; set; }
    }
}
